using WS_4.Models;

namespace WS_4.Com;

public sealed class MachineComComponent : IComComponent
{
    private readonly int _machineId;
    private bool _initialized;
    private bool _disposed;

    public MachineComComponent(int machineId)
    {
        _machineId = machineId;
    }

    public void Initialize()
    {
        if (_initialized)
        {
            return;
        }

        _initialized = true;
    }

    public void Execute(Job job)
    {
        if (_disposed)
        {
            return;
        }

        if (!_initialized)
        {
            Initialize();
        }

        // Simulated COM work; in a real COM server this would invoke native logic.
        _ = _machineId + job.Id;
    }

    public void Dispose()
    {
        _disposed = true;
    }
}
