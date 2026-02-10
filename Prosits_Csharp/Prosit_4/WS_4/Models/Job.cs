using System;
using System.Collections.Generic;
using WS_4.Memory;

namespace WS_4.Models;

public sealed class Job : IDisposable
{
    private readonly MemoryManager _memoryManager;
    private bool _disposed;

    public Job(int id, IReadOnlyList<int> route, int memoryPages, int priority, TimeSpan entryTime, MemoryManager memoryManager)
    {
        Id = id;
        Route = route;
        MemoryPages = memoryPages;
        Priority = priority;
        EntryTime = entryTime;
        _memoryManager = memoryManager;
        CurrentStep = 0;
    }

    public int Id { get; }
    public IReadOnlyList<int> Route { get; }
    public int MemoryPages { get; }
    public int Priority { get; }
    public int CurrentStep { get; private set; }
    public TimeSpan EntryTime { get; }
    public TimeSpan? ExitTime { get; set; }
    public bool IsCrashed { get; set; }

    public int CurrentMachineId => Route[Math.Clamp(CurrentStep, 0, Route.Count - 1)];

    public bool AdvanceRoute(int nextStep)
    {
        CurrentStep = nextStep;
        return CurrentStep >= Route.Count;
    }

    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        // This explicit release simulates long-lived heap objects.
        // If Dispose is skipped, the MemoryManager keeps pages allocated to illustrate GC pressure.
        _memoryManager.Deallocate(Id);
        _disposed = true;
    }
}
