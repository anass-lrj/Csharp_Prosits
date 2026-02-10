using System.Collections.Generic;

namespace WS_4.Models;

public sealed class ProcessingUnitPool
{
    private readonly Queue<ProcessingUnit> _available = new();
    private readonly Queue<Action<ProcessingUnit>> _waiters = new();

    public ProcessingUnitPool(int unitCount)
    {
        for (int i = 0; i < unitCount; i++)
        {
            _available.Enqueue(new ProcessingUnit(i));
        }
    }

    public bool TryAcquire(out ProcessingUnit? unit)
    {
        if (_available.Count == 0)
        {
            unit = null;
            return false;
        }

        unit = _available.Dequeue();
        return true;
    }

    public void RequestUnit(Action<ProcessingUnit> onAcquired)
    {
        if (_available.Count > 0)
        {
            ProcessingUnit unit = _available.Dequeue();
            onAcquired(unit);
            return;
        }

        _waiters.Enqueue(onAcquired);
    }

    public void Release(ProcessingUnit unit)
    {
        if (_waiters.Count > 0)
        {
            Action<ProcessingUnit> waiter = _waiters.Dequeue();
            waiter(unit);
            return;
        }

        _available.Enqueue(unit);
    }

    public int AvailableCount => _available.Count;
}
