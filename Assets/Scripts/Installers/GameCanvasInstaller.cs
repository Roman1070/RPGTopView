using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameCanvasInstaller : MonoInstaller
{
    [SerializeField]
    private GameCanvas _gameCanvas;

    public override void InstallBindings()
    {
        Container.Bind<GameCanvas>().FromInstance(_gameCanvas).AsSingle().NonLazy();
    }
}
