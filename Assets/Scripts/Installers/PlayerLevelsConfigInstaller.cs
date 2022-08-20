using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PlayerLevelsConfigInstaller", menuName = "Installers/PlayerLevelsConfigInstaller")]
public class PlayerLevelsConfigInstaller : ScriptableObjectInstaller<PlayerLevelsConfigInstaller>
{
    [SerializeField]
    private PlayerLevelsConfig _config;

    public override void InstallBindings()
    {
        Container.Bind<PlayerLevelsConfig>().FromScriptableObject(_config).AsSingle().NonLazy();
    }
}