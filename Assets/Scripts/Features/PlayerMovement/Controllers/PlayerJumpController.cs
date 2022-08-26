using DG.Tweening;
using UnityEngine;

public class PlayerJumpController : PlayerMovementControllerBase
{
    private bool _isGrounded;
    private float _stamina;
    private float _speedBeforeJump;
    private bool _groundCheckEnabled;
    private bool _jumpEnabled;
    private Vector3 _velocity;
    private Animator _animator;
    private PlayerMovementConfig _config;

    private bool JumpAvailable => new bool[]
    {
        _stamina >= _config.StaminaOnJump,
        _jumpEnabled,
        !_states.States[PlayerState.Rolling],
        _states.States[PlayerState.Grounded] ,
        !_states.States[PlayerState.Interacting]
    }.Mulitply();

    public PlayerJumpController(PlayerView player, SignalBus signalBus, UpdateProvider updateProvider, PlayerMovementConfig config, PlayerStatesService playerStatesService) : base(player, signalBus, playerStatesService)
    {
        _config = config;
        _animator = _player.Model.GetComponent<Animator>();

        _signalBus.Subscribe<OnInputDataRecievedSignal>(CheckJumpAttempt, this);
        _signalBus.Subscribe<OnPlayerStateChangedSignal>(OnStateChanged, this);
        _signalBus.Subscribe<OnStaminaChangedSignal>(UpdateStamina, this);
        _signalBus.Subscribe<UpdateLastSpeedSignal>(UpdateSpeed, this);
        _groundCheckEnabled = true;
        _jumpEnabled = true;
        updateProvider.Updates.Add(Update);
    }

    private void OnStateChanged(OnPlayerStateChangedSignal obj)
    {
        if (obj.State == PlayerState.Grounded && obj.Value == true)
        {
            _velocity.x = 0;
            _velocity.z = 0;
            DOVirtual.DelayedCall(0.3f, () =>
            {
                _jumpEnabled = true;
            });
        }
    }

    private void UpdateSpeed(UpdateLastSpeedSignal obj)
    {
        _speedBeforeJump = obj.Speed;
    }

    private void UpdateStamina(OnStaminaChangedSignal signal)
    {
        _stamina = signal.Stamina;
    }

    private void CheckJumpAttempt(OnInputDataRecievedSignal signal)
    {
        if (JumpAvailable && signal.Data.JumpAttempt)
        {
            Jump();
        }
    }

    private void Update()
    {
        UpdateGroundedStatus();

        if (_states.States[PlayerState.Grounded] && _velocity.y <= 0)
        {
            _velocity.y = -10f;
        }

        _velocity.y += _config.Gravity * Time.deltaTime;

        _player.Controller.Move(_velocity * Time.deltaTime);
    }

    private void UpdateGroundedStatus()
    {
        if (!_groundCheckEnabled)
        {
            _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Grounded, false));
            return;
        }

        Vector3 origin = _player.GroundChecker.position;
        RaycastHit[] hits = Physics.SphereCastAll(origin, 0.3f, -_player.transform.up, _config.GroundCheckDistance);

        _isGrounded = false;
        foreach (var hit in hits)
        {
            if (hit.collider.CompareTag("Ground"))
            {
                _isGrounded = true;
            }
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
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Grounded, false));
        _groundCheckEnabled = false;
        _jumpEnabled = false;
        DOVirtual.DelayedCall(0.3f, () =>
        {
            _groundCheckEnabled = true;
        });
    }
}
