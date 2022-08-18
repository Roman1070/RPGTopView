using System;
using UnityEngine;

public class PlayerJumpController : PlayerMovementControllerBase
{
    private bool _isGrounded;
    private Vector3 _velocity;
    private PlayerMovementConfig _config;
    private bool _previousGroundedStatus;
    private float _stamina;

    public PlayerJumpController(PlayerView player, SignalBus signalBus, UpdateProvider updateProvider, PlayerMovementConfig config) : base(player, signalBus)
    {
        _config = config;

        _signalBus.Subscribe<OnInputDataRecievedSignal>(CheckJumpAttempt,this);
        _signalBus.Subscribe<OnStaminaChangedSignal>(UpdateStamina,this);
        updateProvider.Updates.Add(Update);
    }

    private void UpdateStamina(OnStaminaChangedSignal signal)
    {
        _stamina = signal.Stamina;
    }

    private void CheckJumpAttempt(OnInputDataRecievedSignal signal)
    {
        if (_isGrounded && signal.Data.JumpAttempt && _stamina >= _config.StaminaOnJump)
        {
            Jump();
        }
    }

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(_player.GroundChecker.position, _config.GroundCheckDistance, LayerMask.GetMask("Ground"));

        if (_isGrounded != _previousGroundedStatus)
        {
            UpdateGroundedStatus();
        }

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += _config.Gravity * Time.deltaTime;

        _player.Controller.Move(_velocity * Time.deltaTime);
        _previousGroundedStatus = _isGrounded;
    }

    private void UpdateGroundedStatus()
    {
        _signalBus.FireSignal(new OnGroundedStatusChangedSignal(_isGrounded));
    }

    private void Jump()
    {
        _velocity.y = Mathf.Sqrt(_config.JumpHeight * -2 * _config.Gravity);
        _signalBus.FireSignal(new OnStaminaChangedSignal(_stamina-_config.StaminaOnJump));
    }
}
