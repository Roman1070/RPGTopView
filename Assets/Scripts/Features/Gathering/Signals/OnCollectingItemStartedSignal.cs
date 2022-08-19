public class OnCollectingItemStartedSignal : ISignal
{
    public CollectableObject Object;
    public OnCollectingItemStartedSignal(CollectableObject obj)
    {
        Object = obj;
    }
}
