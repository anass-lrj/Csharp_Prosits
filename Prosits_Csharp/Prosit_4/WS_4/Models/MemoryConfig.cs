namespace WS_4.Models;

public sealed class MemoryConfig
{
    public int LogicalPages { get; set; } = 4096;
    public int PhysicalFrames { get; set; } = 256;
    public int PageSize { get; set; } = 4096;
    public int PageFaultPenaltyMs { get; set; } = 5;
}
