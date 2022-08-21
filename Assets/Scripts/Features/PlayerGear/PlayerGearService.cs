using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

public class PlayerGearService : LoadableService
{
    private PlayerView _player;
    private List<PlayerGearControllerBase> _controllers;
    private PlayerStatesService _statesService;

    public PlayerGearService(SignalBus signalBus, PlayerView player) : base(signalBus)
    {
        _player = player;
    }

    public override void OnServicesLoaded(params LoadableService[] services)
    {
        _statesService = services.First(s => s.GetType() == typeof(PlayerStatesService)) as PlayerStatesService;
        InitControllers();
    }

    private void InitControllers()
    {
        _controllers = new List<PlayerGearControllerBase>()
        {
            new PlayerArmedStateController(_signalBus,_player,_statesService),
        };
    }
}
