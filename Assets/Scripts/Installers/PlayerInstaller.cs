using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerInstaller : MonoInstaller
{
    [SerializeField]
    private PlayerView _player;

    public override void InstallBindings()
    {
        Container.Bind<PlayerView>().FromInstance(_player).AsSingle().NonLazy();
        Container.QueueForInject(_player);
    }
}
