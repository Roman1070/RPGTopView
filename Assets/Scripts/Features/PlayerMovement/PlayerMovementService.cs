using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerMovementService : LoadableService
{
    private PlayerView _player;
    private PlayerMovementConfig _movementConfig;
    private UpdateProvider _updateProvider;

    private List<PlayerMovementControllerBase> _controllers;

    public PlayerMovementService(SignalBus signalBus, UpdateProvider updateProvider, PlayerView playerView, PlayerMovementConfig config):
        base(signalBus)
    {
        _updateProvider = updateProvider;
           _player = playerView;
        _movementConfig = config;

        InitControllers();
    }

    private void InitControllers()
    {
        _controllers = new List<PlayerMovementControllerBase>()
        {
            new PlayerMovementController(_player,_signalBus,_updateProvider,_movementConfig),
            new PlayerModelRotationController(_player,_signalBus),
            new PlayerJumpController(_player,_signalBus,_updateProvider,_movementConfig),
            new PlayerRollController(_player,_signalBus,_movementConfig,_updateProvider),
            new PlayerStatesController(_player,_signalBus,_updateProvider)
        };
    }
}
