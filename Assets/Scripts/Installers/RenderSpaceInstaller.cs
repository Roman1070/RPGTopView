using UnityEngine;
using Zenject;

public class RenderSpaceInstaller : MonoInstaller
{
    [SerializeField]
    private RenderSpace _renderSpace;

    public override void InstallBindings()
    {
        var renderSpace = GameObject.Instantiate(_renderSpace);
        Container.Bind<RenderSpace>().FromInstance(renderSpace).AsSingle().NonLazy();
        Container.QueueForInject(renderSpace);
    }
}