using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;

namespace WS_4.Models;

public sealed class FactoryConfig
{
    public List<MachineConfig> Machines { get; set; } = new();
    public int ProcessingUnits { get; set; } = 8;
    public MemoryConfig Memory { get; set; } = new();
    public int JobCount { get; set; } = 50;
    public int JobArrivalIntervalMs { get; set; } = 20;
    public int MinJobPages { get; set; } = 4;
    public int MaxJobPages { get; set; } = 24;
    public int RandomSeed { get; set; } = 42;

    public static FactoryConfig LoadOrCreate(string path)
    {
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            FactoryConfig? config = JsonSerializer.Deserialize<FactoryConfig>(json);
            if (config != null)
            {
                return config;
            }
        }

        FactoryConfig created = CreateDefault();
        string output = JsonSerializer.Serialize(created, new JsonSerializerOptions { WriteIndented = true });
        File.WriteAllText(path, output);
        return created;
    }

    public static FactoryConfig CreateDefault()
    {
        FactoryConfig config = new();
        int machineCount = 359;
        for (int i = 1; i <= machineCount; i++)
        {
            config.Machines.Add(new MachineConfig
            {
                Id = i,
                BaseProcessMs = 8 + (i % 5),
                JitterMs = 4
            });
        }

        return config;
    }
}
