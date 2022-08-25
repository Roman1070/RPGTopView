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
        var player = GameObject.Instantiate(_player);
        Container.Bind<PlayerView>().FromInstance(player).AsSingle().NonLazy();
        Container.QueueForInject(player);
    }
}
