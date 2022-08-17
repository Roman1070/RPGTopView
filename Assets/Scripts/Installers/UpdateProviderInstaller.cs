using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class UpdateProviderInstaller : MonoInstaller
{
    [SerializeField]
    private UpdateProvider _updateProvider;

    public override void InstallBindings()
    {
        Container.Bind<UpdateProvider>().FromInstance(_updateProvider).AsSingle().NonLazy();
    }
}
