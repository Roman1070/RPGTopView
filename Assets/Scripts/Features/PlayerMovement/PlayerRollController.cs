using DG.Tweening;
using System;
using UnityEngine;

public class PlayerRollController : PlayerMovementControllerBase
{
    private float _stamina;
    private bool _rollEnabled;
    private Animator _animator;
    private PlayerMovementConfig _config;
    private float _z;
    private Vector3 _rollVector;

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
        if (Mathf.Abs(_z) <= 0.1f)
        {
            return;
        }

        if(_z > 0)
            _z -= Time.deltaTime * _config.RollDistance;
        else
            _z += Time.deltaTime * _config.RollDistance;

        _player.Controller.Move(_rollVector*_z * Time.deltaTime);

    }

    private void Roll(bool forward)
    {
        _z = _config.RollDistance * (forward ? 1 : -1);
        _signalBus.FireSignal(new OnStaminaChangedSignal(_stamina - _config.StaminaOnRoll));
        _animator.SetTrigger(forward ? "Roll forward" : "Roll backward");

        _signalBus.FireSignal(new SetCharacterStateSignal(CharacterState.Rolling, true));
        _rollVector = _player.Model.transform.TransformDirection(new Vector3(0,0,1));
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
