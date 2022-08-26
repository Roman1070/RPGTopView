using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameServicesLoader : MonoBehaviour
{
    #region DEPENDENCIES
    [Inject]
    protected readonly SignalBus _signalBus;
    [Inject]
    protected readonly UpdateProvider _updateProvider;
    [Inject]
    protected readonly PlayerView _playerView;
    [Inject]
    protected readonly PlayerMovementConfig _movementConfig;
    [Inject]
    protected readonly PlayerCombatConfig _combatConfig;
    [Inject]
    protected readonly GameCanvas _gameCanvas;
    [Inject]
    protected readonly ItemsMap _itemsMap;
    [Inject]
    protected readonly PlayerLevelsConfig _levelsConfig;
    [Inject]
    protected readonly CameraMovementConfig _cameraConfig;
    [Inject]
    protected readonly InputConfig _inputConfig;
    [Inject]
    protected readonly EquipedWeaponOffsetConfig _weaponOffsetConfig;
    [Inject]
    protected readonly RenderSpace _playerModel;
    [Inject]
    protected readonly MainCameraAnchor _cameraAnchor;
    #endregion
    private List<LoadableService> _services;

    private void Start()
    {
        InitServices();
    }

    private void InitServices()
    {
        _services = new List<LoadableService>()
        {
            new PlayerMovementService(_signalBus, _updateProvider, _playerView, _movementConfig,_cameraAnchor),
            new GameUiService(_signalBus, _gameCanvas, _movementConfig, _cameraAnchor),
            new DevConsoleService(_signalBus, _gameCanvas),
            new PlayerCombatService(_signalBus, _playerView, _combatConfig, _updateProvider,_cameraAnchor),
            new ItemCollectService(_signalBus, _updateProvider, _playerView),
            new CameraMovementService(_signalBus, _playerView, _cameraConfig, _updateProvider,_cameraAnchor),
            new InventoryService(_signalBus, _itemsMap),
            new InventoryUiService(_signalBus, _gameCanvas),
            new PlayerGearService(_signalBus, _playerView, _weaponOffsetConfig),
            new PlayerModelUpdateService(_signalBus, _playerModel, _playerView, _weaponOffsetConfig),
            new PlayerDataService(_signalBus, _levelsConfig),
            new InputService(_signalBus, _updateProvider, _cameraConfig, _inputConfig),
            new PlayerStatesService(_signalBus),
            new VFXService(_signalBus,_playerView)
        };

        foreach (var service in _services)
            service.OnServicesLoaded(_services.ToArray());
    }
}
