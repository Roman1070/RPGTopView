using DG.Tweening;
using System;
using UnityEngine;

public class PlayerJumpController : PlayerMovementControllerBase
{
    private bool _jumpAvailable;
    private Vector3 _velocity;
    private float _speedBeforeJump;
    private PlayerMovementConfig _config;
    private float _stamina;
    private Animator _animator;
    private bool _previousFrameGrounded;

    public PlayerJumpController(PlayerView player, SignalBus signalBus, UpdateProvider updateProvider, PlayerMovementConfig config) : base(player, signalBus)
    {
        _config = config;
        _animator = _player.Model.GetComponent<Animator>();

        _signalBus.Subscribe<OnInputDataRecievedSignal>(CheckJumpAttempt,this);
        _signalBus.Subscribe<OnStaminaChangedSignal>(UpdateStamina,this);
        _signalBus.Subscribe<SendCharacterStatesSignal>(GetCharacterState, this);
        _signalBus.Subscribe<UpdateLastSpeedSignal>(UpdateSpeed, this);
        updateProvider.Updates.Add(Update);
    }

    private void UpdateSpeed(UpdateLastSpeedSignal obj)
    {
        _speedBeforeJump = obj.Speed;
    }

    private void GetCharacterState(SendCharacterStatesSignal obj)
    {
        _jumpAvailable = _stamina >= _config.StaminaOnJump && !obj.States[PlayerState.Rolling] && obj.States[PlayerState.Grounded];
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
        Vector3 origin = _player.GroundChecker.position;
        RaycastHit[] hits = Physics.SphereCastAll(origin, 0.02f, -_player.transform.up, 0.02f);

        bool isGrounded = false;
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Ground")) isGrounded = true;
        }

        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Grounded, isGrounded));
        if (isGrounded && !_previousFrameGrounded)
        { 
            _velocity.x = 0;
            _velocity.z = 0;
        }

        if (_jumpAvailable && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += _config.Gravity * Time.deltaTime;

        _player.Controller.Move(_velocity * Time.deltaTime);
        _previousFrameGrounded = isGrounded;
    }

    private void Jump()
    {
        _velocity.y = Mathf.Sqrt(_config.JumpHeight * -2 * _config.Gravity);
        _velocity.z = _speedBeforeJump;
        _velocity = _player.Model.transform.TransformDirection(_velocity);
        _signalBus.FireSignal(new OnStaminaChangedSignal(_stamina-_config.StaminaOnJump));
        _animator.SetTrigger("Jump");
    }
}
