using System;
using System.Collections.Generic;
using WS_4.Models;

namespace WS_4.Memory;

// Pedagogical MMU model: logical pages map to a limited set of physical frames with FIFO replacement.
public sealed class MemoryManager
{
    private readonly int _logicalPages;
    private readonly int _physicalFrames;
    private readonly TimeSpan _pageFaultPenalty;
    private readonly PageTable _pageTable = new();
    private Queue<Page> _fifoQueue = new();
    private readonly Dictionary<string, Page> _resident = new();
    private int _allocatedPages;

    public MemoryManager(MemoryConfig config)
    {
        _logicalPages = config.LogicalPages;
        _physicalFrames = config.PhysicalFrames;
        PageSize = config.PageSize;
        _pageFaultPenalty = TimeSpan.FromMilliseconds(config.PageFaultPenaltyMs);
    }

    public int PageSize { get; }
    public int PageFaultCount { get; private set; }
    public int CrashCount { get; private set; }

    public void Allocate(int jobId, int pages)
    {
        if (pages <= 0)
        {
            return;
        }

        if (pages > _logicalPages || _allocatedPages + pages > _logicalPages)
        {
            CrashCount++;
            throw new MemoryAllocationException("Logical memory exhausted.");
        }

        _pageTable.AddJob(jobId, pages);
        _allocatedPages += pages;
    }

    public void Deallocate(int jobId)
    {
        int removed = _pageTable.RemoveJob(jobId);
        if (removed == 0)
        {
            return;
        }

        _allocatedPages -= removed;

        if (_resident.Count == 0)
        {
            return;
        }

        Queue<Page> remaining = new();
        while (_fifoQueue.Count > 0)
        {
            Page page = _fifoQueue.Dequeue();
            if (page.JobId == jobId)
            {
                _resident.Remove(page.Key);
                continue;
            }

            remaining.Enqueue(page);
        }

        _fifoQueue = remaining;
    }

    public TimeSpan TouchPages(int jobId, int pagesToTouch)
    {
        if (!_pageTable.ContainsJob(jobId))
        {
            CrashCount++;
            throw new MemoryAllocationException("Access to non-allocated pages.");
        }

        IReadOnlyList<int> pages = _pageTable.GetPages(jobId);
        int toTouch = Math.Min(pagesToTouch, pages.Count);
        int faults = 0;

        for (int i = 0; i < toTouch; i++)
        {
            Page page = new(jobId, i);
            if (_resident.ContainsKey(page.Key))
            {
                continue;
            }

            faults++;
            if (_resident.Count >= _physicalFrames)
            {
                Page evicted = _fifoQueue.Dequeue();
                _resident.Remove(evicted.Key);
            }

            _resident[page.Key] = page;
            _fifoQueue.Enqueue(page);
        }

        PageFaultCount += faults;
        if (faults == 0)
        {
            return TimeSpan.Zero;
        }

        return TimeSpan.FromMilliseconds(_pageFaultPenalty.TotalMilliseconds * faults);
    }
}
