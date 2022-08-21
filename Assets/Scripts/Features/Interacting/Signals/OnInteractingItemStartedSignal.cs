public class OnInteractingItemStartedSignal : ISignal
{
    public InteractableObject Object;
    public OnInteractingItemStartedSignal(InteractableObject obj)
    {
        Object = obj;
    }
}
