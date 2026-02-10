using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Text.Json;

namespace WS_4.Metrics;

public static class MetricsExporter
{
    public static void WriteJson(MetricsResult result, string path)
    {
        string json = JsonSerializer.Serialize(result, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, json);
    }

    public static void WriteCsv(MetricsResult result, string path)
    {
        StringBuilder sb = new();
        sb.AppendLine("Metric,Value");
        sb.AppendLine($"Scenario,{result.ScenarioName}");
        sb.AppendLine($"TotalSimulatedTimeMs,{result.TotalSimulatedTime.TotalMilliseconds:0.##}");
        sb.AppendLine($"CompletedJobs,{result.CompletedJobs}");
        sb.AppendLine($"CrashedJobs,{result.CrashedJobs}");
        sb.AppendLine($"AverageCycleTimeMs,{result.AverageCycleTimeMs:0.##}");
        sb.AppendLine($"ThroughputPerSecond,{result.ThroughputPerSecond:0.####}");
        sb.AppendLine($"AverageWip,{result.AverageWip:0.####}");
        sb.AppendLine($"PageFaults,{result.PageFaults}");
        sb.AppendLine($"MemoryCrashes,{result.MemoryCrashes}");
        sb.AppendLine($"BottleneckMachineId,{result.BottleneckMachineId}");
        sb.AppendLine($"BottleneckUtilization,{result.BottleneckUtilization:0.####}");

        sb.AppendLine();
        sb.AppendLine("MachineId,Utilization");
        foreach (KeyValuePair<int, double> entry in result.UtilizationByMachine)
        {
            sb.AppendLine($"{entry.Key},{entry.Value:0.####}");
        }

        File.WriteAllText(path, sb.ToString());
    }
}
