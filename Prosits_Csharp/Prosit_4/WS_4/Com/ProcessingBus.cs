using System.Collections.Generic;
using WS_4.Models;

namespace WS_4.Com;

public sealed class ProcessingBus
{
    private readonly ComComponentFactory _factory;
    private readonly Dictionary<int, IComComponent> _components = new();

    public ProcessingBus(ComComponentFactory factory)
    {
        _factory = factory;
    }

    public void Execute(int machineId, Job job)
    {
        if (!_components.TryGetValue(machineId, out IComComponent? component))
        {
            component = _factory.Create(machineId);
            component.Initialize();
            _components[machineId] = component;
        }

        component.Execute(job);
    }

    public void DisposeAll()
    {
        foreach (IComComponent component in _components.Values)
        {
            component.Dispose();
        }

        _components.Clear();
    }
}
