using System.Collections.Generic;

public enum PlayerState
{
    Running,
    Rolling,
    Jumping,
    Attacking,
    Collecting
}

public class PlayerStatesController : PlayerMovementControllerBase
{
    public Dictionary<PlayerState, bool> States;

    public PlayerStatesController(PlayerView player, SignalBus signalBus, UpdateProvider updateProvider) : base(player, signalBus)
    {
        States = new Dictionary<PlayerState, bool>()
        {
            {PlayerState.Running, false},
            {PlayerState.Rolling, false},
            {PlayerState.Jumping, false},
            {PlayerState.Attacking, false},
            {PlayerState.Collecting, false},
        };
        _signalBus.Subscribe<SetPlayerStateSignal>(SetState, this);
        updateProvider.Updates.Add(Update);
    }

    private void Update()
    {
        _signalBus.FireSignal(new SendCharacterStatesSignal(States));
    }

    private void SetState(SetPlayerStateSignal signal)
    {
        States[signal.State] = signal.Value;
    }
}
