using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

public class PlayerMovementService : LoadableService
{
    private PlayerView _player;
    private PlayerMovementConfig _movementConfig;
    private UpdateProvider _updateProvider;
    private PlayerStatesService _playerStatesService;

    private List<PlayerMovementControllerBase> _controllers;

    public PlayerMovementService(SignalBus signalBus, UpdateProvider updateProvider, PlayerView playerView, PlayerMovementConfig config):
        base(signalBus)
    {
        _updateProvider = updateProvider;
           _player = playerView;
        _movementConfig = config;
    }

    public override void OnServicesLoaded(params LoadableService[] services)
    {
        _playerStatesService = services.First(s => s.GetType() == typeof(PlayerStatesService)) as PlayerStatesService;
        InitControllers();
    }

    private void InitControllers()
    {
        _controllers = new List<PlayerMovementControllerBase>()
        {
            new PlayerMovementController(_player,_signalBus,_updateProvider,_movementConfig,_playerStatesService),
            new PlayerModelRotationController(_player,_signalBus,_playerStatesService),
            new PlayerJumpController(_player,_signalBus,_updateProvider,_movementConfig,_playerStatesService),
            new PlayerRollController(_player,_signalBus,_movementConfig,_updateProvider,_playerStatesService),
            new PlayerStaminaController(_player, _signalBus, _movementConfig, _updateProvider,_playerStatesService),
            new PlayerRotationController(_player,_signalBus,_playerStatesService)
        };
    }
}
