using System.Collections;
using UnityEngine;

public class ItemCollectController : ItemInteractControllerBase
{
    public ItemCollectController(SignalBus signalBus, PlayerView player) : base(signalBus,player)
    {
    }

    protected override void Interact(InteractableObject obj)
    {
        if (obj is CollectableObject)
        {
            _animator.SetTrigger("Collect");
            obj = obj as CollectableObject;
            _player.StartCoroutine(InteractProcess(obj));
        }
    }

    protected override void OnCollected(InteractableObject obj)
    {
        _signalBus.FireSignal(new OnItemCountChangedSignal((obj as CollectableObject).Items));
    }
}
