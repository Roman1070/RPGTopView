using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class PlayerMovementService : LoadableService
{
    [Inject]
    private PlayerView _player;
    [Inject]
    private UpdateProvider _updateProvider;
    [Inject]
    private PlayerMovementConfig _movementConfig;
    [Inject]
    private PlayerCombatConfig _combatConfig;

    private List<PlayerMovementControllerBase> _controllers;

    public override void Init()
    {
        base.Init();
        InitControllers();
    }

    private void InitControllers()
    {
        _controllers = new List<PlayerMovementControllerBase>()
        {
            new PlayerMovementController(_player,_signalBus,_updateProvider,_movementConfig),
            new PlayerModelRotationController(_player,_signalBus),
            new PlayerJumpController(_player,_signalBus,_updateProvider,_movementConfig),
            new PlayerCombatController(_player,_signalBus,_combatConfig)
        };
    }
}
