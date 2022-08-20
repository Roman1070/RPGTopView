using DG.Tweening;
using UnityEngine;

public class PlayerJumpController : PlayerMovementControllerBase
{
    private bool _isGrounded;
    private bool _jumpAvailable;
    private bool _previousFrameGrounded;
    private bool _jumpedRecently;
    private float _stamina;
    private float _speedBeforeJump;
    private Vector3 _velocity;
    private Animator _animator;
    private PlayerMovementConfig _config;

    public PlayerJumpController(PlayerView player, SignalBus signalBus, UpdateProvider updateProvider, PlayerMovementConfig config) : base(player, signalBus)
    {
        _config = config;
        _animator = _player.Model.GetComponent<Animator>();

        _signalBus.Subscribe<OnInputDataRecievedSignal>(CheckJumpAttempt, this);
        _signalBus.Subscribe<OnStaminaChangedSignal>(UpdateStamina, this);
        _signalBus.Subscribe<SendPlayerStatesSignal>(GetCharacterState, this);
        _signalBus.Subscribe<UpdateLastSpeedSignal>(UpdateSpeed, this);
        updateProvider.Updates.Add(Update);
    }

    private void UpdateSpeed(UpdateLastSpeedSignal obj)
    {
        _speedBeforeJump = obj.Speed;
    }

    private void GetCharacterState(SendPlayerStatesSignal obj)
    {
        _jumpAvailable = _stamina >= _config.StaminaOnJump && !obj.States[PlayerState.Rolling] && obj.States[PlayerState.Grounded]
            && !obj.States[PlayerState.Collecting] &&!_jumpedRecently;
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
        UpdateGroundedStatus();

        if (_isGrounded && !_previousFrameGrounded)
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
        _previousFrameGrounded = _isGrounded;
    }

    private void UpdateGroundedStatus()
    {
        Vector3 origin = _player.GroundChecker.position;
        RaycastHit[] hits = Physics.SphereCastAll(origin, 0.2f, -_player.transform.up, 0.2f);

        _isGrounded = false;
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Ground")) _isGrounded = true;
        }

        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Grounded, _isGrounded));
    }

    private void Jump()
    {
        _velocity.y = Mathf.Sqrt(_config.JumpHeight * -2 * _config.Gravity);
        _velocity.z = _speedBeforeJump;
        _velocity = _player.Model.transform.TransformDirection(_velocity);
        _signalBus.FireSignal(new OnStaminaChangedSignal(_stamina - _config.StaminaOnJump));
        _animator.SetTrigger("Jump");
        _jumpedRecently = true;
        DOVirtual.DelayedCall(1.3f, () =>
        {
            _jumpedRecently = false;
        });
    }
}
