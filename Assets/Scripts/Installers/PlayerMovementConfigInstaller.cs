using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PlayerMovementConfigInstaller" , menuName = "Installers/PlayerMovementConfigInstaller")]
public class PlayerMovementConfigInstaller : ScriptableObjectInstaller
{
    [SerializeField]
    private PlayerMovementConfig _Config;

    public override void InstallBindings()
    {
        Container.Bind<PlayerMovementConfig>().FromScriptableObject(_Config).AsSingle().NonLazy();
    }
}

