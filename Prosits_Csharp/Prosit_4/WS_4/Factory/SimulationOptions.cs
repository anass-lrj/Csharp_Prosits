using WS_4.Models;

namespace WS_4.Factory;

public sealed class SimulationOptions
{
    public string ScenarioName { get; set; } = string.Empty;
    public MemoryConfig? MemoryOverride { get; set; }
    public bool AutoDisposeJobs { get; set; } = true;
}
