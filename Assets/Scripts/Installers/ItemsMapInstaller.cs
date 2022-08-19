using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "ItemsMapInstaller", menuName = "Installers/ItemsMapInstaller")]
public class ItemsMapInstaller : ScriptableObjectInstaller<ItemsMapInstaller>
{
    [SerializeField]
    private ItemsMap _itemsMap;

    public override void InstallBindings()
    {
        Container.Bind<ItemsMap>().FromScriptableObject(_itemsMap).AsSingle().NonLazy();
    }
}