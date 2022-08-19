using DG.Tweening;
using System;
using UnityEngine;

public class PlayerRollController : PlayerMovementControllerBase
{
    private float _stamina;
    private bool _rollEnabled;
    private Animator _animator;
    private PlayerMovementConfig _config;
    private Vector3 _velocity;

    public PlayerRollController(PlayerView player, SignalBus signalBus, PlayerMovementConfig config, UpdateProvider updateProvider) : base(player, signalBus)
    {
        _config = config;
        updateProvider.Updates.Add(Update);
        _signalBus.Subscribe<OnStaminaChangedSignal>(UpdateStamina, this);
        _signalBus.Subscribe<OnInputDataRecievedSignal>(CheckRollAttempt, this);
        _signalBus.Subscribe<GetCharacterStatesSignal>(GetCharacterStates, this);
        _animator = _player.Model.GetComponent<Animator>();
    }

    private void GetCharacterStates(GetCharacterStatesSignal obj)
    {
        _rollEnabled = !(obj.States[CharacterState.Jumping] || obj.States[CharacterState.Rolling]);
    }

    private void CheckRollAttempt(OnInputDataRecievedSignal signal)
    {
        if (_rollEnabled && signal.Data.RollAttempt && _stamina >= _config.StaminaOnRoll)
        {
            Roll(signal.Data.Direction.y >= 0);
        }
    }

    private void Update()
    {
        if (Mathf.Abs(_velocity.z) <= 0.1f)
        {
            return;
        }

        if(_velocity.z>0)
            _velocity.z -= Time.deltaTime * _config.RollDistance;
        else
            _velocity.z += Time.deltaTime * _config.RollDistance;

        _player.Controller.Move(_player.Model.transform.TransformDirection(_velocity) * Time.deltaTime);

    }

    private void Roll(bool forward)
    {
        _velocity.z = _config.RollDistance * (forward ? 1 : -1);
        _signalBus.FireSignal(new OnStaminaChangedSignal(_stamina - _config.StaminaOnRoll));
        _animator.SetTrigger(forward ? "Roll forward" : "Roll backward");

        _signalBus.FireSignal(new SetCharacterStateSignal(CharacterState.Rolling, true));

        DOVirtual.DelayedCall(0.8f, () =>
        {
            _signalBus.FireSignal(new SetCharacterStateSignal(CharacterState.Rolling, false));
        });
    }

    private void UpdateStamina(OnStaminaChangedSignal signal)
    {
        _stamina = signal.Stamina;
    }
}
