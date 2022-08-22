using System;
using System.Collections.Generic;
using System.Linq;

public class ItemCollectService : LoadableService
{
    private PlayerView _player;
    private UpdateProvider _updateProvider;
    private PlayerStatesService _playerStatesService;

    private List<ItemInteractControllerBase> _controllers;

    public ItemCollectService(SignalBus signalBus, UpdateProvider updateProvider, PlayerView playerView) : base(signalBus)
    {
        _updateProvider = updateProvider;
        _player = playerView;
    }

    public override void OnServicesLoaded(params LoadableService[] services)
    {
        _playerStatesService = services.First(s => s.GetType() == typeof(PlayerStatesService)) as PlayerStatesService;
        InitControllers();
    }

    private void InitControllers()
    {
        new EnvironmentItemCheckController(_signalBus, _updateProvider, _player);
        new ItemInteractAttemptController(_signalBus, _playerStatesService);
        _controllers = new List<ItemInteractControllerBase>()
        {
            new ItemCollectController(_signalBus,_player),
            new ShrineInteractController(_signalBus,_player,_playerStatesService)
        };
    }
}
