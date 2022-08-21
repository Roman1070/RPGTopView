using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerCombatService : LoadableService
{
    private PlayerView _player;
    private PlayerCombatConfig _config;
    private List<PlayerCombatControllerBase> _controllers;
    private PlayerStatesService _playerStatesService;

    public PlayerCombatService(SignalBus signalBus, PlayerView player, PlayerCombatConfig config)
        : base(signalBus)
    {
        _player = player;
        _config = config;
        signalBus.Subscribe<OnServicesLoadedSignal>(OnServicesLoaded, this);
    }

    private void OnServicesLoaded(OnServicesLoadedSignal obj)
    {
        _playerStatesService = obj.Services.First(s => s is PlayerStatesService) as PlayerStatesService;
        InitControllers();
    }

    private void InitControllers()
    {
        _controllers = new List<PlayerCombatControllerBase>()
        {
            new PlayerCombatController(_player,_signalBus,_config,_playerStatesService),
        };
    }
}
