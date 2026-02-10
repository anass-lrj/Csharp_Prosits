using System;
using System.Collections.Generic;
using System.Linq;
using WS_4.Memory;
using WS_4.Metrics;
using WS_4.Models;

namespace WS_4.Factory;

public static class JobGenerator
{
    public static List<Job> GenerateJobs(FactoryConfig config, MemoryManager memoryManager, MetricsCollector metrics)
    {
        List<Job> jobs = new();
        Random random = new(config.RandomSeed);
        List<int> route = config.Machines.Select(m => m.Id).ToList();

        for (int i = 0; i < config.JobCount; i++)
        {
            int pages = random.Next(config.MinJobPages, config.MaxJobPages + 1);
            int priority = random.Next(1, 10);
            TimeSpan entryTime = TimeSpan.FromMilliseconds(i * config.JobArrivalIntervalMs);
            Job job = new(i + 1, route, pages, priority, entryTime, memoryManager);

            try
            {
                memoryManager.Allocate(job.Id, job.MemoryPages);
            }
            catch (MemoryAllocationException)
            {
                // Allocation failure is a simulated crash before entering the system.
                metrics.RecordPreEntryCrash();
                continue;
            }

            jobs.Add(job);
        }

        return jobs;
    }
}
