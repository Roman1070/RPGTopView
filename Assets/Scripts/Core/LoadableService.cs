using UnityEngine;
using Zenject;

public abstract class LoadableService
{
    protected SignalBus _signalBus;

    public LoadableService(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
}