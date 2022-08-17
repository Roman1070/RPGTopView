using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameUiService : LoadableService
{
    private List<GameUiControllerBase> _controllers;

    public override void Init()
    {
        _controllers = new List<GameUiControllerBase>()
        {
            new StaminaSliderController(_signalBus),
        };
    }
}
