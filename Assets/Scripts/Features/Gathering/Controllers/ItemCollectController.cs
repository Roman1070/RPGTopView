using System.Collections;
using UnityEngine;

public class ItemCollectController : ItemCollectControllerBase
{
    private Animator _animator;
    private PlayerView _player;

    public ItemCollectController(SignalBus signalBus, PlayerView player) : base(signalBus)
    {
        _player = player;
        _animator = player.Model.GetComponent<Animator>();

        signalBus.Subscribe<OnCollectingItemStartedSignal>(OnCollectingStarted,this);
    }

    private void OnCollectingStarted(OnCollectingItemStartedSignal signal)
    {
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Collecting, true));
        _animator.SetTrigger("Collect");
        _player.StartCoroutine(FinishCollecting(signal.Object,_collectingTime));
    }

    private IEnumerator FinishCollecting(CollectableObject obj, float delay)
    {
        yield return new WaitForSeconds(delay);
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Collecting, false));
        _signalBus.FireSignal(new OnItemCountChangedSignal(obj.Id, obj.Count));
    }
}
