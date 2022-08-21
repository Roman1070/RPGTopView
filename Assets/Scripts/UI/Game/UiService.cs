using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class UiService : LoadableService
{
    private GameCanvas _gameCanvas;
    private PlayerMovementConfig _movementConfig;
    private Camera _playerCamera;
    private PlayerStatesService _statesService;
    private InputService _inputService;
    private InventoryService _inventory;

    private List<GameUiControllerBase> _controllers;

    public UiService(SignalBus signalBus, GameCanvas canvas, PlayerMovementConfig config, Camera playerCamera) : base(signalBus)
    {
        _gameCanvas = canvas;
        _movementConfig = config;
        _playerCamera = playerCamera;
    }

    public override void OnServicesLoaded(params LoadableService[] services)
    {
        _inputService = services.First(s => s.GetType() == typeof(InputService)) as InputService;
        _statesService = services.First(s => s.GetType() == typeof(PlayerStatesService)) as PlayerStatesService;
        _inventory = services.First(s => s.GetType() == typeof(InventoryService)) as InventoryService;
        InitControllers();
    }

    public void InitControllers()
    {
        _controllers = new List<GameUiControllerBase>()
        {
            new StaminaSliderController(_signalBus,_gameCanvas,_movementConfig),
            new CollectedItemWidgetsController(_signalBus,_gameCanvas, _inventory),
            new InteractButtonController(_signalBus, _gameCanvas,_playerCamera,_statesService,_inputService),
            new PlayerExperienceUiController(_signalBus,_gameCanvas),
            new UiPanelsController(_signalBus, _gameCanvas)
        };
    }
}
