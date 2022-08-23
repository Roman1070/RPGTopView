using UnityEngine;
using Zenject;

public class PlayerModelForRenderingInstaller : MonoInstaller
{
    [SerializeField]
    private PlayerModelForRendering _model;

    public override void InstallBindings()
    {
        Container.Bind<PlayerModelForRendering>().FromInstance(_model).AsSingle().NonLazy();
    }
}