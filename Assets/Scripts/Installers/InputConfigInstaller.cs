using UnityEngine;
using Zenject;

[CreateAssetMenu(fileName = "InputConfigInstaller", menuName = "Installers/InputConfigInstaller")]
public class InputConfigInstaller : ScriptableObjectInstaller<InputConfigInstaller>
{
    [SerializeField]
    private InputConfig _config;

    public override void InstallBindings()
    {
        Container.Bind<InputConfig>().FromScriptableObject(_config).AsSingle().NonLazy();
    }
}