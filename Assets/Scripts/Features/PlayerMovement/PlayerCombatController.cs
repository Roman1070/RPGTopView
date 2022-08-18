using DG.Tweening;
using UnityEngine;

public class PlayerCombatController : PlayerMovementControllerBase
{
    private PlayerCombatConfig _config;
    private Animator _animator;
    private bool _attackAvailable;

    public PlayerCombatController(PlayerView player, SignalBus signalBus, PlayerCombatConfig config) : base(player, signalBus)
    {
        signalBus.Subscribe<OnInputDataRecievedSignal>(OnInputRecieved, this);
        _animator = player.Model.GetComponent<Animator>();
        _attackAvailable = true;
        _config = config;
    }

    private void OnInputRecieved(OnInputDataRecievedSignal signal)
    {
        if (signal.Data.AttackAttempt &&_attackAvailable)
        {
            _animator.SetTrigger("Attack");
            _attackAvailable = false;
            _signalBus.FireSignal(new OnMovementAbilityStatusChangedSignal(false));
            float attackDuration = _config.AttacksDurations[0] + _config.AttacksDurations[1] + _config.AttacksDurations[2];

            DOVirtual.DelayedCall(attackDuration,()=>
            {
                _attackAvailable = true;
                _signalBus.FireSignal(new OnMovementAbilityStatusChangedSignal(true));
            });
        }
    }
}
