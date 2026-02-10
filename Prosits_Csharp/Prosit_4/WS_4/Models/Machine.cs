using System;
using System.Collections.Generic;
using WS_4.Com;
using WS_4.Engine;
using WS_4.Memory;
using WS_4.Metrics;

namespace WS_4.Models;

public sealed class Machine
{
    private readonly SimulationEngine _engine;
    private readonly MemoryManager _memoryManager;
    private readonly ProcessingUnitPool _unitPool;
    private readonly ProcessingBus _processingBus;
    private readonly MetricsCollector _metrics;
    private readonly Func<int, Machine?> _machineResolver;
    private readonly ProcessingTimeDelegate _processingTime;
    private readonly JobPriorityDelegate _prioritySelector;
    private readonly JobRoutingDelegate _routing;
    private readonly bool _autoDisposeJobs;
    private readonly PriorityQueue<Job, int> _queue = new();
    private bool _waitingForUnit;

    public Machine(
        int id,
        SimulationEngine engine,
        MemoryManager memoryManager,
        ProcessingUnitPool unitPool,
        ProcessingBus processingBus,
        MetricsCollector metrics,
        Func<int, Machine?> machineResolver,
        ProcessingTimeDelegate processingTime,
        JobPriorityDelegate prioritySelector,
        JobRoutingDelegate routing,
        bool autoDisposeJobs)
    {
        Id = id;
        _engine = engine;
        _memoryManager = memoryManager;
        _unitPool = unitPool;
        _processingBus = processingBus;
        _metrics = metrics;
        _machineResolver = machineResolver;
        _processingTime = processingTime;
        _prioritySelector = prioritySelector;
        _routing = routing;
        _autoDisposeJobs = autoDisposeJobs;
        State = MachineState.Idle;
    }

    public int Id { get; }
    public MachineState State { get; private set; }

    public void AcceptJob(Job job)
    {
        _queue.Enqueue(job, _prioritySelector(job));
        TryStartNext();
    }

    private void TryStartNext()
    {
        if (State != MachineState.Idle || _waitingForUnit)
        {
            return;
        }

        if (!_queue.TryDequeue(out Job? job, out _))
        {
            return;
        }

        _waitingForUnit = true;
        _unitPool.RequestUnit(unit =>
        {
            _waitingForUnit = false;
            StartProcessing(job, unit);
        });
    }

    private void StartProcessing(Job job, ProcessingUnit unit)
    {
        State = MachineState.Busy;
        _metrics.RecordMachineStart(Id, _engine.CurrentTime);

        try
        {
            _processingBus.Execute(Id, job);
            TimeSpan baseDuration = _processingTime(job);
            TimeSpan memoryPenalty = _memoryManager.TouchPages(job.Id, job.MemoryPages);
            TimeSpan totalDuration = baseDuration + memoryPenalty;

            _engine.ScheduleIn(totalDuration, () => FinishProcessing(job, unit));
        }
        catch (MemoryAllocationException)
        {
            _metrics.RecordJobCrash(job, _engine.CurrentTime);
            if (_autoDisposeJobs)
            {
                job.Dispose();
            }

            _unitPool.Release(unit);
            State = MachineState.Idle;
            _metrics.RecordMachineStop(Id, _engine.CurrentTime);
            TryStartNext();
        }
    }

    private void FinishProcessing(Job job, ProcessingUnit unit)
    {
        _unitPool.Release(unit);
        _metrics.RecordMachineStop(Id, _engine.CurrentTime);
        State = MachineState.Idle;

        int nextStep = _routing(job);
        bool completed = job.AdvanceRoute(nextStep);

        if (completed)
        {
            job.ExitTime = _engine.CurrentTime;
            _metrics.RecordJobCompletion(job);
            if (_autoDisposeJobs)
            {
                job.Dispose();
            }
        }
        else
        {
            int nextMachineId = job.Route[job.CurrentStep];
            Machine? nextMachine = _machineResolver(nextMachineId);
            if (nextMachine == null)
            {
                _metrics.RecordJobCrash(job, _engine.CurrentTime);
            }
            else
            {
                _engine.Schedule(_engine.CurrentTime, () => nextMachine.AcceptJob(job));
            }
        }

        TryStartNext();
    }
}
