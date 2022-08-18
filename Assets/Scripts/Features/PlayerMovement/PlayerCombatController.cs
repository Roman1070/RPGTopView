using UnityEngine;

public class PlayerCombatController : PlayerMovementControllerBase
{
    private Animator _animator;

    public PlayerCombatController(PlayerView player, SignalBus signalBus) : base(player, signalBus)
    {
        signalBus.Subscribe<OnInputDataRecievedSignal>(OnInputRecieved, this);
        _animator = player.Model.GetComponent<Animator>();
    }

    private void OnInputRecieved(OnInputDataRecievedSignal signal)
    {
        if (signal.Data.AttackAttempt)
        {
            _animator.SetTrigger("Attack");
        }
    }
}
