using System;
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
    #endregion

    #region SINGLETONE
    private static ServicesLoader _instance;
    public static ServicesLoader Instance
    {
        get
        {
            if (_instance == null)
                _instance = FindObjectOfType<ServicesLoader>();
            return _instance;
        }
    }
    #endregion

    private List<LoadableService> _services;

    private void Start()
    {
        DontDestroyOnLoad(gameObject);

        _services = new List<LoadableService>()
        {
            new InputService(_signalBus,_updateProvider),
            new PlayerMovementService(_signalBus,_updateProvider,_playerView,_movementConfig),
            new GameUiService(_signalBus,_gameCanvas,_movementConfig),
            new PlayerCombatService(_signalBus,_playerView,_combatConfig),
            new InventoryService(_signalBus,_itemsMap),
            new ItemCollectService(_signalBus,_updateProvider,_playerView),
        };
    }

    public LoadableService GetService(Type type)
    {
        foreach(var service in _services)
        {
            if (service.GetType() == type)
                return service;
        }
        return null;
    }
}
