using UnityEngine;
using Zenject;

public abstract class LoadableService : MonoBehaviour
{
    [Inject]
    protected readonly SignalBus _signalBus;

    public virtual void Init() { }
}