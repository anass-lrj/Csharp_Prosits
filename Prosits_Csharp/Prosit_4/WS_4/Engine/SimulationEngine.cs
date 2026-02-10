using System;

namespace WS_4.Engine;

public sealed class SimulationEngine
{
    private readonly EventQueue _queue = new();
    private readonly Action<TimeSpan>? _onTimeAdvanced;
    private long _sequence;

    public SimulationEngine(Action<TimeSpan>? onTimeAdvanced = null)
    {
        _onTimeAdvanced = onTimeAdvanced;
    }

    public TimeSpan CurrentTime { get; private set; } = TimeSpan.Zero;

    public void Schedule(TimeSpan time, Action action)
    {
        if (time < CurrentTime)
        {
            throw new ArgumentException("Cannot schedule an event in the past.", nameof(time));
        }

        _queue.Enqueue(new SimEvent(time, action, _sequence++));
    }

    public void ScheduleIn(TimeSpan delay, Action action)
    {
        Schedule(CurrentTime + delay, action);
    }

    public void Run()
    {
        while (_queue.Count > 0)
        {
            SimEvent next = _queue.Dequeue();
            AdvanceTime(next.Time);
            next.Action();
        }
    }

    private void AdvanceTime(TimeSpan newTime)
    {
        if (newTime < CurrentTime)
        {
            return;
        }

        CurrentTime = newTime;
        _onTimeAdvanced?.Invoke(CurrentTime);
    }
}
