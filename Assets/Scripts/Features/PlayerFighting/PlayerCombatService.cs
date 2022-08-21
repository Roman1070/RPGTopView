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
    }

    public override void OnServicesLoaded(params LoadableService[] services)
    {
        _playerStatesService = services.First(s => s.GetType() == typeof(PlayerStatesService)) as PlayerStatesService;
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
