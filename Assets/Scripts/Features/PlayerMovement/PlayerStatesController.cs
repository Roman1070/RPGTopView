using System.Collections.Generic;

public enum CharacterState
{
    Running,
    Rolling,
    Jumping,
    Attacking
}

public class PlayerStatesController : PlayerMovementControllerBase
{
    public Dictionary<CharacterState, bool> States;

    public PlayerStatesController(PlayerView player, SignalBus signalBus, UpdateProvider updateProvider) : base(player, signalBus)
    {
        States = new Dictionary<CharacterState, bool>()
        {
            {CharacterState.Running, false},
            {CharacterState.Rolling, false},
            {CharacterState.Jumping, false},
            {CharacterState.Attacking, false},
        };
        _signalBus.Subscribe<SetCharacterStateSignal>(SetState, this);
        updateProvider.Updates.Add(Update);
    }

    private void Update()
    {
        _signalBus.FireSignal(new GetCharacterStatesSignal(States));
    }

    private void SetState(SetCharacterStateSignal signal)
    {
        States[signal.State] = signal.Value;
    }
}
