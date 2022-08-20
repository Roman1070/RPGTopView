using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameUiService : LoadableService
{
    private GameCanvas _gameCanvas;
    private PlayerMovementConfig _movementConfig;
    private Camera _playerCamera;

    private List<GameUiControllerBase> _controllers;

    public GameUiService(SignalBus signalBus, GameCanvas canvas, PlayerMovementConfig config, Camera playerCamera) : base(signalBus)
    {
        _gameCanvas = canvas;
        _movementConfig = config;
        _playerCamera = playerCamera;
        InitControllers();
    }

    public void InitControllers()
    {
        _controllers = new List<GameUiControllerBase>()
        {
            new StaminaSliderController(_signalBus,_gameCanvas,_movementConfig),
            new CollectedItemWidgetsController(_signalBus,_gameCanvas),
            new CollectButtonController(_signalBus, _gameCanvas,_playerCamera),
            new PlayerExperienceUiController(_signalBus,_gameCanvas)
        };
    }
}
