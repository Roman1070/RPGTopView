using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class MainCameraAnchorInstaller : MonoInstaller
{
    [SerializeField]
    private MainCameraAnchor _instance;

    public override void InstallBindings()
    {
        Container.Bind<MainCameraAnchor>().FromInstance(_instance).AsSingle().NonLazy();
    }
}
