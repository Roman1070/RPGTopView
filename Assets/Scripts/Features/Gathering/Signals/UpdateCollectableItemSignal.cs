using UnityEngine;

public class UpdateCollectableItemSignal : ISignal
{
    public CollectableObject Object;

    public UpdateCollectableItemSignal(CollectableObject obj)
    {
        Object = obj;
    }
}
