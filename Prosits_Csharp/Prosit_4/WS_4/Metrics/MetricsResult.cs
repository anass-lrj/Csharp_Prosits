using System;
using System.Collections.Generic;

namespace WS_4.Metrics;

public sealed class MetricsResult
{
    public string ScenarioName { get; set; } = string.Empty;
    public TimeSpan TotalSimulatedTime { get; set; }
    public int CompletedJobs { get; set; }
    public int CrashedJobs { get; set; }
    public double AverageCycleTimeMs { get; set; }
    public double ThroughputPerSecond { get; set; }
    public double AverageWip { get; set; }
    public Dictionary<int, double> UtilizationByMachine { get; set; } = new();
    public int PageFaults { get; set; }
    public int MemoryCrashes { get; set; }
    public int BottleneckMachineId { get; set; }
    public double BottleneckUtilization { get; set; }
}
