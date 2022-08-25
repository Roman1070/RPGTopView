using System.Collections.Generic;

public enum PlayerState
{
    Idle,
    Running,
    Rolling,
    Attacking,
    Interacting,
    Grounded,
    IsArmed,
    DrawingWeapon,
    Dodging,
    BrowsingUI,
}

public class PlayerStatesService : LoadableService
{
    public Dictionary<PlayerState, bool> States { get; private set; }

    public PlayerStatesService(SignalBus signalBus) : base(signalBus)
    {
        States = new Dictionary<PlayerState, bool>()
        {
            {PlayerState.Idle, true},
            {PlayerState.Running, false},
            {PlayerState.Rolling, false},
            {PlayerState.Attacking, false},
            {PlayerState.Interacting, false},
            {PlayerState.Grounded, true},
            {PlayerState.IsArmed, false},
            {PlayerState.DrawingWeapon, false},
            {PlayerState.Dodging, false},
            {PlayerState.BrowsingUI, false},
        };
        _signalBus.Subscribe<SetPlayerStateSignal>(SetState, this);
    }

    private void SetState(SetPlayerStateSignal signal)
    {
        if (States[signal.State] != signal.Value)
            _signalBus.FireSignal(new OnPlayerStateChangedSignal(signal.State, signal.Value));

        States[signal.State] = signal.Value;
    }

    public override void OnServicesLoaded(params LoadableService[] services)
    {
    }
}
