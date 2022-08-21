public class PlayerMovementControllerBase
{
    protected SignalBus _signalBus;
    protected PlayerView _player;
    protected PlayerStatesService _states;

    public PlayerMovementControllerBase(PlayerView player, SignalBus signalBus, PlayerStatesService playerStatesService)
    {
        _player = player;
        _signalBus = signalBus;
        _states = playerStatesService;
    }

}
