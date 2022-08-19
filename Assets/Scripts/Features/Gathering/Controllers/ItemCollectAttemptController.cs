
using System;

public class ItemCollectAttemptController : ItemCollectControllerBase
{
    private bool _collectAttempt;

    private bool _collectingAvailable;

    public ItemCollectAttemptController(SignalBus signalBus) : base(signalBus)
    {
        _signalBus.Subscribe<UpdateCollectableItemSignal>(UpdateCollectablesData, this);
        _signalBus.Subscribe<OnInputDataRecievedSignal>(UpdateInputData,this);
        _signalBus.Subscribe<SendCharacterStatesSignal>(UpdatePlayerState, this);
    }

    private void UpdatePlayerState(SendCharacterStatesSignal signal)
    {
        _collectingAvailable = !(signal.States[PlayerState.Rolling] || signal.States[PlayerState.Jumping]
            || signal.States[PlayerState.Collecting] || signal.States[PlayerState.Attacking]);
    }

    private void UpdateInputData(OnInputDataRecievedSignal obj)
    {
        _collectAttempt = obj.Data.CollectAttempt;
    }

    private void UpdateCollectablesData(UpdateCollectableItemSignal signal)
    {
        if (_collectAttempt && _collectingAvailable)
        {
            if (signal.Object != null)
            {
                _signalBus.FireSignal(new OnCollectingItemStartedSignal(signal.Object));
            }
        }
    }
}
