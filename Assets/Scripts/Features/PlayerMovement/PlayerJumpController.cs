using DG.Tweening;
using System;
using UnityEngine;

public class PlayerJumpController : PlayerMovementControllerBase
{
    private bool _jumpAvailable;
    private bool _isJumping;
    private Vector3 _velocity;
    private PlayerMovementConfig _config;
    private float _stamina;
    private Animator _animator;

    public PlayerJumpController(PlayerView player, SignalBus signalBus, UpdateProvider updateProvider, PlayerMovementConfig config) : base(player, signalBus)
    {
        _config = config;
        _animator = _player.Model.GetComponent<Animator>();

        _signalBus.Subscribe<OnInputDataRecievedSignal>(CheckJumpAttempt,this);
        _signalBus.Subscribe<OnStaminaChangedSignal>(UpdateStamina,this);
        _signalBus.Subscribe<GetCharacterStatesSignal>(GetCharacterState, this);
        //_signalBus.Subscribe<UpdateVelocityBeforeJumpSignal>(UpdateVelocity, this);
        updateProvider.Updates.Add(Update);
    }

    private void UpdateVelocity(UpdateVelocityBeforeJumpSignal obj)
    {
        _velocity = obj.Velocity;
    }

    private void GetCharacterState(GetCharacterStatesSignal obj)
    {
        _isJumping = obj.States[CharacterState.Jumping];

        _jumpAvailable = _stamina >= _config.StaminaOnJump && !(obj.States[CharacterState.Rolling] || _isJumping);
    }

    private void UpdateStamina(OnStaminaChangedSignal signal)
    {
        _stamina = signal.Stamina;
    }

    private void CheckJumpAttempt(OnInputDataRecievedSignal signal)
    {
        if (_jumpAvailable && signal.Data.JumpAttempt)
        {
            Jump();
        }
    }

    private void Update()
    {
        if (!_isJumping && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += _config.Gravity * Time.deltaTime;

        _player.Controller.Move(_velocity * Time.deltaTime);
    }

    private void Jump()
    {
        _velocity.y = Mathf.Sqrt(_config.JumpHeight * -2 * _config.Gravity);
        _signalBus.FireSignal(new OnStaminaChangedSignal(_stamina-_config.StaminaOnJump));
        _animator.SetTrigger("Jump");
        _signalBus.FireSignal(new SetCharacterStateSignal(CharacterState.Jumping, true));
        DOVirtual.DelayedCall(0.9f, () =>
        {
            _signalBus.FireSignal(new SetCharacterStateSignal(CharacterState.Jumping, false));
        });
    }
}
