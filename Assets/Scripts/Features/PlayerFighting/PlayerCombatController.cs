using DG.Tweening;
using System;
using UnityEngine;

public class PlayerCombatController : PlayerCombatControllerBase
{
    private PlayerStatesService _states;
    private Animator _animator;
    private bool _attackAvailable;

    public PlayerCombatController(PlayerView player, SignalBus signalBus, PlayerCombatConfig config, PlayerStatesService states) : base(signalBus, player, config)
    {
        _animator = player.Model.GetComponent<Animator>();
        _config = config;
        _states = states;
        signalBus.Subscribe<OnInputDataRecievedSignal>(OnInputRecieved, this);
    }

    private void OnInputRecieved(OnInputDataRecievedSignal signal)
    {
        _attackAvailable = !(!_states.States[PlayerState.Grounded] || _states.States[PlayerState.Rolling] || _states.States[PlayerState.Attacking]);

        if (signal.Data.AttackAttempt &&_attackAvailable)
        {
            _animator.SetTrigger("Attack");
            _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Attacking, true));
            float attackDuration = _config.AttacksDurations[0];

            DOVirtual.DelayedCall(attackDuration,()=>
            {
                _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Attacking, false));
            });
        }
    }
}
