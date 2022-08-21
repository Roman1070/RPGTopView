using System;
using System.Collections.Generic;

public class PlayerDataService : LoadableService
{
    private List<PlayerDataControllerBase> _controllers;
    private PlayerLevelsConfig _levelsConfig;

    public PlayerDataService(SignalBus signalBus, PlayerLevelsConfig levelConfig) : base(signalBus)
    {
        _levelsConfig = levelConfig;
        InitControllers();
    }

    public override void OnServicesLoaded(params LoadableService[] services)
    {

    }

    private void InitControllers()
    {
        _controllers = new List<PlayerDataControllerBase>()
        {
            new PlayerExpirienceController(_signalBus,_levelsConfig)
        };
    }
}
