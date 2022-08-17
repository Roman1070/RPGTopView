using UnityEngine;

public class PlayerJumpController : PlayerMovementControllerBase
{
    private float _jumpHeight = 1.5f;
    private float _gravity = -9.81f;
    private bool _jumpAttempt;
    private bool _isGrounded;
    private float _groundCheckDistance = 0.15f;
    private Vector3 _velocity;

    private bool _previousGroundedStatus;

    public PlayerJumpController(PlayerView player, SignalBus signalBus, UpdateProvider updateProvider) : base(player, signalBus)
    {
        _signalBus.Subscribe<OnInputDataRecievedSignal>(UpdateJumpAttempt,this);
        updateProvider.Updates.Add(Update);
    }

    private void UpdateJumpAttempt(OnInputDataRecievedSignal signal)
    {
        _jumpAttempt = signal.Jump;
    }

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(_player.GroundChecker.position, _groundCheckDistance, LayerMask.GetMask("Ground"));

        if (_isGrounded != _previousGroundedStatus)
        {
            UpdateGroundedStatus();
        }

        if (_isGrounded && _jumpAttempt)
        {
            Jump();
        }

        if (_isGrounded && _velocity.y < 0)
        {
            _velocity.y = -2f;
        }

        _velocity.y += _gravity * Time.deltaTime;

        _player.Controller.Move(_velocity * Time.deltaTime);
        _previousGroundedStatus = _isGrounded;
    }

    private void UpdateGroundedStatus()
    {
        _signalBus.FireSignal(new OnGroundedStatusChangedSignal(_isGrounded));
    }

    private void Jump()
    {
        _velocity.y = Mathf.Sqrt(_jumpHeight * -2 * _gravity);
    }
}
