using System;
using System.Collections.Generic;
using WS_4.Com;
using WS_4.Engine;
using WS_4.Memory;
using WS_4.Metrics;
using WS_4.Models;

namespace WS_4.Factory;

public sealed class FactorySimulation
{
    private readonly FactoryConfig _config;

    public FactorySimulation(FactoryConfig config)
    {
        _config = config;
    }

    public MetricsResult RunScenario(SimulationOptions options)
    {
        MemoryConfig memoryConfig = options.MemoryOverride ?? _config.Memory;
        MetricsCollector metrics = new();
        MemoryManager memoryManager = new(memoryConfig);
        SimulationEngine engine = new(metrics.OnTimeAdvanced);
        ProcessingUnitPool unitPool = new(_config.ProcessingUnits);
        ProcessingBus bus = new(new ComComponentFactory());

        Dictionary<int, Machine> machines = new();

        JobPriorityDelegate prioritySelector = job => -job.Priority;
        JobRoutingDelegate routing = job => job.CurrentStep + 1;

        foreach (MachineConfig machineConfig in _config.Machines)
        {
            ProcessingTimeDelegate processingTime = job =>
            {
                int jitterRange = Math.Max(1, machineConfig.JitterMs + 1);
                int jitter = (job.Id + machineConfig.Id) % jitterRange;
                return TimeSpan.FromMilliseconds(machineConfig.BaseProcessMs + jitter);
            };

            Machine machine = new(
                machineConfig.Id,
                engine,
                memoryManager,
                unitPool,
                bus,
                metrics,
                id => machines.TryGetValue(id, out Machine? found) ? found : null,
                processingTime,
                prioritySelector,
                routing,
                options.AutoDisposeJobs);

            machines[machineConfig.Id] = machine;
        }

        List<Job> jobs = JobGenerator.GenerateJobs(_config, memoryManager, metrics);

        foreach (Job job in jobs)
        {
            int firstMachineId = job.Route[0];
            if (!machines.TryGetValue(firstMachineId, out Machine? firstMachine))
            {
                continue;
            }

            engine.Schedule(job.EntryTime, () =>
            {
                metrics.RecordJobEntered(job);
                firstMachine.AcceptJob(job);
            });
        }

        engine.Run();
        bus.DisposeAll();

        return metrics.BuildResult(options.ScenarioName, engine.CurrentTime, memoryManager.PageFaultCount, memoryManager.CrashCount);
    }
}
