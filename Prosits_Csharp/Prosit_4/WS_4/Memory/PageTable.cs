using System.Collections.Generic;

namespace WS_4.Memory;

public sealed class PageTable
{
    private readonly Dictionary<int, List<int>> _jobPages = new();

    public bool ContainsJob(int jobId) => _jobPages.ContainsKey(jobId);

    public void AddJob(int jobId, int pages)
    {
        List<int> list = new(pages);
        for (int i = 0; i < pages; i++)
        {
            list.Add(i);
        }

        _jobPages[jobId] = list;
    }

    public IReadOnlyList<int> GetPages(int jobId)
    {
        return _jobPages[jobId];
    }

    public int RemoveJob(int jobId)
    {
        if (!_jobPages.TryGetValue(jobId, out List<int>? pages))
        {
            return 0;
        }

        _jobPages.Remove(jobId);
        return pages.Count;
    }
}
