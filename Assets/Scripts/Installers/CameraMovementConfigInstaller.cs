using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "CameraMovementConfigInstaller", menuName = "Installers/CameraMovementConfigInstaller")]
public class CameraMovementConfigInstaller : ScriptableObjectInstaller<CameraMovementConfigInstaller>
{
    [SerializeField]
    private CameraMovementConfig _config;

    public override void InstallBindings()
    {
        Container.Bind<CameraMovementConfig>().FromScriptableObject(_config).AsSingle().NonLazy();
    }
}