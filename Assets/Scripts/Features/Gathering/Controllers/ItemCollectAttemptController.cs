
using System;

public class ItemCollectAttemptController : ItemCollectControllerBase
{
    private PlayerStatesService _states;

    private bool _collectAttempt;

    private bool _collectingAvailable;

    public ItemCollectAttemptController(SignalBus signalBus, PlayerStatesService states) : base(signalBus)
    {
        _states = states;

        _signalBus.Subscribe<UpdateCollectableItemSignal>(UpdateCollectablesData, this);
        _signalBus.Subscribe<OnInputDataRecievedSignal>(UpdateInputData, this);
    }

    private void UpdateInputData(OnInputDataRecievedSignal obj)
    {
        _collectAttempt = obj.Data.CollectAttempt;
    }

    private void UpdateCollectablesData(UpdateCollectableItemSignal signal)
    {
        _collectingAvailable = !(_states.States[PlayerState.Rolling] || !_states.States[PlayerState.Grounded]
               || _states.States[PlayerState.Collecting] || _states.States[PlayerState.Attacking]);

        if (_collectAttempt && _collectingAvailable)
        {
            if (signal.Object != null)
            {
                _signalBus.FireSignal(new OnCollectingItemStartedSignal(signal.Object));
            }
        }
    }
}
