using System.Collections.Generic;

public class PlayerCombatService : LoadableService
{
    private PlayerView _player;
    private PlayerCombatConfig _config;
    private List<PlayerCombatControllerBase> _controllers;

    public PlayerCombatService(SignalBus signalBus, PlayerView player, PlayerCombatConfig config)
        : base(signalBus)
    {
        _player = player;
        _config = config;
        InitControllers();
    }
    private void InitControllers()
    {
        _controllers = new List<PlayerCombatControllerBase>()
        {
            new PlayerCombatController(_player,_signalBus,_config),
        };
    }
}
