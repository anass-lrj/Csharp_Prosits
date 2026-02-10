namespace WS_4.Memory;

public sealed class Page
{
    public Page(int jobId, int pageId)
    {
        JobId = jobId;
        PageId = pageId;
    }

    public int JobId { get; }
    public int PageId { get; }

    public string Key => $"{JobId}:{PageId}";
}
