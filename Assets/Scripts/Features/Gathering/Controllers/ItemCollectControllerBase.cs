public class ItemCollectControllerBase 
{
    protected SignalBus _signalBus;
    protected float _collectingTime = 1.8f;

    public ItemCollectControllerBase(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
}
