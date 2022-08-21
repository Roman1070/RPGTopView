using UnityEngine;

public class UpdateInteractableItemSignal : ISignal
{
    public InteractableObject Object;

    public UpdateInteractableItemSignal(InteractableObject obj)
    {
        Object = obj;
    }
}
