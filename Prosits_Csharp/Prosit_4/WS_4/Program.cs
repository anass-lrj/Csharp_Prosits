using System;
using System.IO;
using WS_4.Factory;
using WS_4.Metrics;
using WS_4.Models;

namespace WS_4;

public static class Program
{
    public static void Main()
    {
        string baseDir = Directory.GetCurrentDirectory();
        string configPath = Path.Combine(baseDir, "factoryConfig.json");
        FactoryConfig config = FactoryConfig.LoadOrCreate(configPath);

        string outputDir = Path.Combine(baseDir, "output");
        Directory.CreateDirectory(outputDir);

        FactorySimulation simulation = new(config);

        SimulationOptions normal = new()
        {
            ScenarioName = "Normal",
            AutoDisposeJobs = true
        };

        SimulationOptions limitedMemory = new()
        {
            ScenarioName = "MemoryLimited",
            AutoDisposeJobs = true,
            MemoryOverride = new MemoryConfig
            {
                LogicalPages = config.Memory.LogicalPages,
                PhysicalFrames = Math.Max(8, config.Memory.PhysicalFrames / 8),
                PageSize = config.Memory.PageSize,
                PageFaultPenaltyMs = config.Memory.PageFaultPenaltyMs * 4
            }
        };

        SimulationOptions badDispose = new()
        {
            ScenarioName = "BadDispose",
            AutoDisposeJobs = false,
            MemoryOverride = new MemoryConfig
            {
                LogicalPages = Math.Max(64, config.MinJobPages * (config.JobCount / 2)),
                PhysicalFrames = config.Memory.PhysicalFrames,
                PageSize = config.Memory.PageSize,
                PageFaultPenaltyMs = config.Memory.PageFaultPenaltyMs
            }
        };

        RunScenario(simulation, normal, outputDir);
        RunScenario(simulation, limitedMemory, outputDir);
        RunScenario(simulation, badDispose, outputDir);

        Console.WriteLine("Simulation completed. KPI exports are in the output folder.");
    }

    private static void RunScenario(FactorySimulation simulation, SimulationOptions options, string outputDir)
    {
        Console.WriteLine();
        Console.WriteLine($"--- Scenario: {options.ScenarioName} ---");
        Console.WriteLine("Note: This pedagogical model uses IDisposable to simulate heap pressure when jobs are not released.");
        Console.WriteLine("Note: This is a discrete-event mock; it is not a real OS or COM runtime.");

        MetricsResult result = simulation.RunScenario(options);
        PrintResult(result);

        string jsonPath = Path.Combine(outputDir, $"kpi_{options.ScenarioName}.json");
        string csvPath = Path.Combine(outputDir, $"kpi_{options.ScenarioName}.csv");
        MetricsExporter.WriteJson(result, jsonPath);
        MetricsExporter.WriteCsv(result, csvPath);
    }

    private static void PrintResult(MetricsResult result)
    {
        Console.WriteLine($"Simulated time: {result.TotalSimulatedTime.TotalMilliseconds:0.##} ms");
        Console.WriteLine($"Completed jobs: {result.CompletedJobs}");
        Console.WriteLine($"Crashed jobs: {result.CrashedJobs}");
        Console.WriteLine($"Average cycle time: {result.AverageCycleTimeMs:0.##} ms");
        Console.WriteLine($"Throughput: {result.ThroughputPerSecond:0.####} jobs/s");
        Console.WriteLine($"Average WIP: {result.AverageWip:0.####}");
        Console.WriteLine($"Page faults: {result.PageFaults}");
        Console.WriteLine($"Memory crashes: {result.MemoryCrashes}");
        Console.WriteLine($"Bottleneck machine: {result.BottleneckMachineId} ({result.BottleneckUtilization:P1})");
    }
}
