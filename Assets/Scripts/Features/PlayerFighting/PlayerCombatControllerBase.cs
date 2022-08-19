public class PlayerCombatControllerBase
{
    protected SignalBus _signalBus;
    protected PlayerView _player;
    protected PlayerCombatConfig _config;

    public PlayerCombatControllerBase(SignalBus signalBus, PlayerView player, PlayerCombatConfig config)
    {
        _signalBus = signalBus;
        _player = player;
        _config = config;
    }
}
