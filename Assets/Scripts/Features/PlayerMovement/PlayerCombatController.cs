using DG.Tweening;
using System;
using UnityEngine;

public class PlayerCombatController : PlayerMovementControllerBase
{
    private PlayerCombatConfig _config;
    private Animator _animator;
    private bool _attackAvailable;

    public PlayerCombatController(PlayerView player, SignalBus signalBus, PlayerCombatConfig config) : base(player, signalBus)
    {
        signalBus.Subscribe<OnInputDataRecievedSignal>(OnInputRecieved, this);
        signalBus.Subscribe<GetCharacterStatesSignal>(GetCharacterStates, this);
        _animator = player.Model.GetComponent<Animator>();
        _signalBus.FireSignal(new SetCharacterStateSignal(CharacterState.Attacking, false));
        _config = config;
    }

    private void GetCharacterStates(GetCharacterStatesSignal obj)
    {
        _attackAvailable = !(obj.States[CharacterState.Jumping] || obj.States[CharacterState.Rolling] || obj.States[CharacterState.Attacking]);
    }

    private void OnInputRecieved(OnInputDataRecievedSignal signal)
    {
        if (signal.Data.AttackAttempt &&_attackAvailable)
        {
            _animator.SetTrigger("Attack");
            _signalBus.FireSignal(new SetCharacterStateSignal(CharacterState.Attacking, true));
            float attackDuration = _config.AttacksDurations[0];

            DOVirtual.DelayedCall(attackDuration,()=>
            {
                _signalBus.FireSignal(new SetCharacterStateSignal(CharacterState.Attacking, false));
            });
        }
    }
}
