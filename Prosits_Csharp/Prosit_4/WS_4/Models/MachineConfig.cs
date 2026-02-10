namespace WS_4.Models;

public sealed class MachineConfig
{
    public int Id { get; set; }
    public int BaseProcessMs { get; set; } = 10;
    public int JitterMs { get; set; } = 5;
}
