public class PlayerDataControllerBase
{
    protected SignalBus _signalBus;

    public PlayerDataControllerBase(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
}

public class PlayerExpirienceController : PlayerDataControllerBase
{
    public PlayerExpirienceController(SignalBus signalBus) : base(signalBus)
    {
    }
}
