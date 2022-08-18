using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "PlayerCombatConfigInstaller", menuName = "Installers/PlayerCombatConfigInstaller")]
public class PlayerCombatConfigInstaller : ScriptableObjectInstaller
{
    [SerializeField]
    private PlayerCombatConfig _Config;

    public override void InstallBindings()
    {
        Container.Bind<PlayerCombatConfig>().FromScriptableObject(_Config).AsSingle().NonLazy();
    }
}
