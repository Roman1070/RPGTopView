public class PlayerGearControllerBase
{
    protected SignalBus _signalBus;
    protected PlayerView _player;

    public PlayerGearControllerBase(SignalBus signalBus, PlayerView player)
    {
        _signalBus = signalBus;
        _player = player;
    }
}