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

    private List<GameUiControllerBase> _controllers;

    public UiService(SignalBus signalBus, GameCanvas canvas, PlayerMovementConfig config, Camera playerCamera) : base(signalBus)
    {
        _gameCanvas = canvas;
        _movementConfig = config;
        _playerCamera = playerCamera;
        signalBus.Subscribe<OnServicesLoadedSignal>(OnServicesLoaded,this);
    }

    private void OnServicesLoaded(OnServicesLoadedSignal obj)
    {
        _statesService = obj.Services.First(s => s is PlayerStatesService) as PlayerStatesService;
        _inputService = obj.Services.First(s => s is InputService) as InputService;
        InitControllers();
    }

    public void InitControllers()
    {
        _controllers = new List<GameUiControllerBase>()
        {
            new StaminaSliderController(_signalBus,_gameCanvas,_movementConfig),
            new CollectedItemWidgetsController(_signalBus,_gameCanvas),
            new CollectButtonController(_signalBus, _gameCanvas,_playerCamera,_statesService,_inputService),
            new PlayerExperienceUiController(_signalBus,_gameCanvas),
            new UiPanelsController(_signalBus, _gameCanvas)
        };
    }
}
