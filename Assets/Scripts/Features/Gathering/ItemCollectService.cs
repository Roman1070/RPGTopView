using System.Collections.Generic;

public class ItemCollectService : LoadableService
{
    private PlayerView _player;
    private UpdateProvider _updateProvider;

    private List<ItemCollectControllerBase> _controllers;

    public ItemCollectService(SignalBus signalBus, UpdateProvider updateProvider, PlayerView playerView) : base(signalBus)
    {
        _updateProvider = updateProvider;
        _player = playerView;
        InitControllers();
    }

    private void InitControllers()
    {
        _controllers = new List<ItemCollectControllerBase>()
        {
            new EnvironmentItemCheckController(_signalBus,_updateProvider,_player),
            new ItemCollectAttemptController(_signalBus),
            new ItemCollectController(_signalBus,_player)
        };
    }
}
