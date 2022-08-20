using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameUiService : LoadableService
{
    private GameCanvas _gameCanvas;
    private PlayerMovementConfig _movementConfig;

    private List<GameUiControllerBase> _controllers;

    public GameUiService(SignalBus signalBus, GameCanvas canvas, PlayerMovementConfig config) : base(signalBus)
    {
        _gameCanvas = canvas;
        _movementConfig = config;

        InitControllers();
    }

    public void InitControllers()
    {
        _controllers = new List<GameUiControllerBase>()
        {
            new StaminaSliderController(_signalBus,_gameCanvas,_movementConfig),
            new CollectedItemWidgetsController(_signalBus,_gameCanvas),
        };
    }
}
