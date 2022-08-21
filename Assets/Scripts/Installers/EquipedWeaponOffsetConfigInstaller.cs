using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "EquipedWeaponOffsetConfigInstaller", menuName = "Installers/EquipedWeaponOffsetConfigInstaller")]
public class EquipedWeaponOffsetConfigInstaller : ScriptableObjectInstaller<EquipedWeaponOffsetConfigInstaller>
{
    [SerializeField]
    private EquipedWeaponOffsetConfig _config;

    public override void InstallBindings()
    {
        Container.Bind<EquipedWeaponOffsetConfig>().FromScriptableObject(_config).AsSingle().NonLazy();
    }
}