using DG.Tweening;
using System;
using UnityEngine;

public class PlayerCombatController : PlayerCombatControllerBase
{
    private Animator _animator;
    private bool _attackAvailable;

    public PlayerCombatController(PlayerView player, SignalBus signalBus, PlayerCombatConfig config) : base( signalBus,player,config)
    {
        signalBus.Subscribe<OnInputDataRecievedSignal>(OnInputRecieved, this);
        signalBus.Subscribe<SendCharacterStatesSignal>(GetCharacterStates, this);
        _animator = player.Model.GetComponent<Animator>();
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Attacking, false));
        _config = config;
    }

    private void GetCharacterStates(SendCharacterStatesSignal obj)
    {
        _attackAvailable = !(!obj.States[PlayerState.Grounded] || obj.States[PlayerState.Rolling] || obj.States[PlayerState.Attacking]);
    }

    private void OnInputRecieved(OnInputDataRecievedSignal signal)
    {
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
