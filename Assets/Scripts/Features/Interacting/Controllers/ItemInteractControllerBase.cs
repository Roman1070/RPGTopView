using System;
using System.Collections;
using UnityEngine;

public abstract class ItemInteractControllerBase
{
    protected SignalBus _signalBus;
    protected Animator _animator;
    protected PlayerView _player;

    public ItemInteractControllerBase(SignalBus signalBus, PlayerView player)
    {
        _signalBus = signalBus;
        _player = player;
        _animator = _player.Model.GetComponent<Animator>();

        signalBus.Subscribe<OnInteractingItemStartedSignal>(OnInteractingStarted, this);
    }

    private void OnInteractingStarted(OnInteractingItemStartedSignal signal)
    {
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Interacting, true));
        CheckObjectType(signal.Object);
    }

    protected abstract void CheckObjectType(InteractableObject obj);

    protected IEnumerator Interact(InteractableObject obj)
    {
        yield return new WaitForSeconds(obj.InteractionTime);
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Interacting, false));
        obj.OnInteractingFinished();
        OnCollected(obj);
    }

    protected abstract void OnCollected(InteractableObject obj);
}
