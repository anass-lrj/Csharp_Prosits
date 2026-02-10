using System;

namespace WS_4.Memory;

public sealed class MemoryAllocationException : Exception
{
    public MemoryAllocationException(string message) : base(message)
    {
    }
}
