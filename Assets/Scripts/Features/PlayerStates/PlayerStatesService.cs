using System.Collections.Generic;
using UnityEngine;

public enum PlayerState
{
    Running,
    Rolling,
    Attacking,
    Collecting,
    Grounded
}

public class PlayerStatesService : LoadableService
{
    public Dictionary<PlayerState, bool> States { get; private set; }

    public PlayerStatesService(SignalBus signalBus) : base(signalBus)
    {
        States = new Dictionary<PlayerState, bool>()
        {
            {PlayerState.Running, false},
            {PlayerState.Rolling, false},
            {PlayerState.Attacking, false},
            {PlayerState.Collecting, false},
            {PlayerState.Grounded, true},
        };
        _signalBus.Subscribe<SetPlayerStateSignal>(SetState, this);
    }

    private void SetState(SetPlayerStateSignal signal)
    {
        if (States[signal.State] != signal.Value)
            _signalBus.FireSignal(new OnPlayerStateChangedSignal(signal.State, signal.Value));

        States[signal.State] = signal.Value;
    }

    public bool GetState(PlayerState state) => States[state];
}
