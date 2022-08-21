﻿using System;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class ServicesLoader : MonoBehaviour
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
    #endregion

    private List<LoadableService> _services;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _services = new List<LoadableService>()
        {
            new InputService(_signalBus,_updateProvider,_cameraConfig,_inputConfig),
            new PlayerMovementService(_signalBus,_updateProvider,_playerView,_movementConfig),
            new UiService(_signalBus,_gameCanvas,_movementConfig, _playerView.Camera),
            new PlayerCombatService(_signalBus,_playerView,_combatConfig),
            new ItemCollectService(_signalBus,_updateProvider,_playerView),
            new InventoryService(_signalBus,_itemsMap),
            new DevConsoleService(_signalBus, _gameCanvas),
            new PlayerDataService(_signalBus, _levelsConfig),
            new PlayerStatesService(_signalBus),
            new CameraMovementService(_signalBus,_playerView,_cameraConfig),
            new InventoryUiService(_signalBus, _gameCanvas)
        };
        _signalBus.FireSignal(new OnServicesLoadedSignal(_services));
    }
}