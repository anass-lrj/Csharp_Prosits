using System;
using System.Collections.Generic;
using WS_4.Models;

namespace WS_4.Metrics;

public sealed class MetricsCollector
{
    private readonly Dictionary<int, MachineMetrics> _machines = new();
    private long _wipAreaTicks;
    private TimeSpan _lastTime = TimeSpan.Zero;
    private int _currentWip;
    private int _completedJobs;
    private int _crashedJobs;
    private long _cycleTimeTicks;

    public void OnTimeAdvanced(TimeSpan now)
    {
        if (now <= _lastTime)
        {
            return;
        }

        TimeSpan delta = now - _lastTime;
        _wipAreaTicks += delta.Ticks * _currentWip;
        _lastTime = now;
    }

    public void RecordJobEntered(Job job)
    {
        _currentWip++;
    }

    public void RecordJobCompletion(Job job)
    {
        _completedJobs++;
        _currentWip = Math.Max(0, _currentWip - 1);
        if (job.ExitTime.HasValue)
        {
            _cycleTimeTicks += (job.ExitTime.Value - job.EntryTime).Ticks;
        }
    }

    public void RecordJobCrash(Job job, TimeSpan time)
    {
        job.IsCrashed = true;
        _crashedJobs++;
        _currentWip = Math.Max(0, _currentWip - 1);
    }

    public void RecordPreEntryCrash()
    {
        _crashedJobs++;
    }

    public void RecordMachineStart(int machineId, TimeSpan time)
    {
        MachineMetrics metrics = GetMachineMetrics(machineId);
        metrics.LastStart = time;
    }

    public void RecordMachineStop(int machineId, TimeSpan time)
    {
        MachineMetrics metrics = GetMachineMetrics(machineId);
        if (metrics.LastStart.HasValue)
        {
            metrics.BusyTicks += (time - metrics.LastStart.Value).Ticks;
            metrics.LastStart = null;
        }
    }

    public MetricsResult BuildResult(string scenarioName, TimeSpan totalTime, int pageFaults, int memoryCrashes)
    {
        double totalSeconds = Math.Max(1.0, totalTime.TotalSeconds);
        double avgCycleMs = _completedJobs == 0 ? 0 : TimeSpan.FromTicks(_cycleTimeTicks / _completedJobs).TotalMilliseconds;
        double avgWip = totalTime.Ticks == 0 ? 0 : (double)_wipAreaTicks / totalTime.Ticks;

        Dictionary<int, double> utilization = new();
        int bottleneckId = 0;
        double bottleneckUtil = 0;

        foreach ((int id, MachineMetrics metrics) in _machines)
        {
            double util = totalTime.Ticks == 0 ? 0 : (double)metrics.BusyTicks / totalTime.Ticks;
            utilization[id] = util;
            if (util > bottleneckUtil)
            {
                bottleneckUtil = util;
                bottleneckId = id;
            }
        }

        return new MetricsResult
        {
            ScenarioName = scenarioName,
            TotalSimulatedTime = totalTime,
            CompletedJobs = _completedJobs,
            CrashedJobs = _crashedJobs,
            AverageCycleTimeMs = avgCycleMs,
            ThroughputPerSecond = _completedJobs / totalSeconds,
            AverageWip = avgWip,
            UtilizationByMachine = utilization,
            PageFaults = pageFaults,
            MemoryCrashes = memoryCrashes,
            BottleneckMachineId = bottleneckId,
            BottleneckUtilization = bottleneckUtil
        };
    }

    private MachineMetrics GetMachineMetrics(int machineId)
    {
        if (!_machines.TryGetValue(machineId, out MachineMetrics? metrics))
        {
            metrics = new MachineMetrics();
            _machines[machineId] = metrics;
        }

        return metrics;
    }

    private sealed class MachineMetrics
    {
        public long BusyTicks { get; set; }
        public TimeSpan? LastStart { get; set; }
    }
}
