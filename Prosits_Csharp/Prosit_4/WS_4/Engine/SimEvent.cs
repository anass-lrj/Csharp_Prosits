using System;

namespace WS_4.Engine;

public sealed class SimEvent
{
    public SimEvent(TimeSpan time, Action action, long sequence)
    {
        Time = time;
        Action = action;
        Sequence = sequence;
    }

    public TimeSpan Time { get; }
    public Action Action { get; }
    public long Sequence { get; }
}
