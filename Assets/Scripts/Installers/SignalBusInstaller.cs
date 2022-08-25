using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class SignalBusInstaller : MonoInstaller
{
    [SerializeField]
    private SignalBus _signalBus;

    public override void InstallBindings()
    {
        Container.Bind<SignalBus>().FromComponentInNewPrefab(_signalBus).AsSingle();
        Container.QueueForInject(_signalBus);
    }
}
