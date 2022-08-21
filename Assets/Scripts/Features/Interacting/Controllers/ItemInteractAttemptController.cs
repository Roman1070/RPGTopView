public class ItemInteractAttemptController
{
    private SignalBus _signalBus;
    private PlayerStatesService _states;

    private bool _collectAttempt;

    private bool _interactingAvailable;

    public ItemInteractAttemptController(SignalBus signalBus, PlayerStatesService states)
    {
        _states = states;
        _signalBus = signalBus;
        _signalBus.Subscribe<UpdateInteractableItemSignal>(UpdateCollectablesData, this);
        _signalBus.Subscribe<OnInputDataRecievedSignal>(UpdateInputData, this);
    }

    private void UpdateInputData(OnInputDataRecievedSignal obj)
    {
        _collectAttempt = obj.Data.CollectAttempt;
    }

    private void UpdateCollectablesData(UpdateInteractableItemSignal signal)
    {
        _interactingAvailable = !(_states.States[PlayerState.Rolling] || !_states.States[PlayerState.Grounded]
               || _states.States[PlayerState.Interacting] || _states.States[PlayerState.Attacking]);

        if (_collectAttempt && _interactingAvailable)
        {
            if (signal.Object != null)
            {
                if (signal.Object is CollectableObject) _signalBus.FireSignal(new OnInteractingItemStartedSignal(signal.Object as CollectableObject));
                if (signal.Object is InteractableShrine) _signalBus.FireSignal(new OnInteractingItemStartedSignal(signal.Object as InteractableShrine));
            }
        }
    }
}
