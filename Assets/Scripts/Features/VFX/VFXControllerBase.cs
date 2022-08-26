using System;

public class VFXControllerBase
{
    protected SignalBus _signalBus;

    public VFXControllerBase(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
}
