using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class GameUiService : LoadableService
{
    [Inject]
    private GameCanvas _gameCanvas;
    [Inject]
    private PlayerMovementConfig _movementConfig;

    private List<GameUiControllerBase> _controllers;

    public override void Init()
    {
        _controllers = new List<GameUiControllerBase>()
        {
            new StaminaSliderController(_signalBus,_gameCanvas,_movementConfig),
        };
    }
}
