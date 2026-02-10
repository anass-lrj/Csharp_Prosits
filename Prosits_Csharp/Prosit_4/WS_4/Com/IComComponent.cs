using System;
using WS_4.Models;

namespace WS_4.Com;

public interface IComComponent : IDisposable
{
    void Initialize();
    void Execute(Job job);
}
