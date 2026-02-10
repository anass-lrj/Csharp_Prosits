namespace WS_4.Com;

public sealed class ComComponentFactory
{
    public IComComponent Create(int machineId)
    {
        return new MachineComComponent(machineId);
    }
}
