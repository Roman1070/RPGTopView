using DG.Tweening;
using UnityEngine;

public class PlayerMovementController : PlayerMovementControllerBase
{
    private float _speed;
    private float _stamina;
    private bool _movementAvailable;
    private PlayerMovementConfig _config;
    private Animator _animator;

    public PlayerMovementController(PlayerView player, SignalBus signalBus, UpdateProvider updateProvider, PlayerMovementConfig config, PlayerStatesService statesService) : base(player, signalBus, statesService)
    {
        _config = config;
        _animator = _player.Model.GetComponent<Animator>();

        _signalBus.Subscribe<OnInputDataRecievedSignal>(OnInputRecieved, this);
        _signalBus.Subscribe<OnStaminaChangedSignal>(OnStaminaChanged, this);

        Cursor.lockState = CursorLockMode.Locked;
        _stamina = _config.MaxStamina;
    }

    private void OnStaminaChanged(OnStaminaChangedSignal signal)
    {
        _stamina = signal.Stamina;
        if (_stamina <= 0) EndRun();
    }

    private void OnInputRecieved(OnInputDataRecievedSignal signal)
    {
        _movementAvailable = !(!_states.States[PlayerState.Grounded] || _states.States[PlayerState.Rolling]
            || _states.States[PlayerState.Interacting]);

        var moveDirection = new Vector3(signal.Data.Direction.x, 0, signal.Data.Direction.y);
        moveDirection = _player.transform.TransformDirection(moveDirection);

        if (!_states.States[PlayerState.Running] && signal.Data.SprintAttempt && signal.Data.Direction.y >= 0 && _movementAvailable && _stamina > 10)
            StartRun();

        if (_states.States[PlayerState.Running] && (signal.Data.SprintBreak || signal.Data.Direction.y < 0 || signal.Data.Direction == Vector2.zero))
            EndRun();

        if (_movementAvailable)
        {
            _player.Controller.Move(moveDirection * _speed * Time.deltaTime);
            _player.transform.Rotate(Vector3.up, signal.Data.Rotation.x);
            if (signal.Data.Direction == Vector2.zero)
            {
                _signalBus.FireSignal(new UpdateLastSpeedSignal(0));
            }
            else
            {
                _signalBus.FireSignal(new UpdateLastSpeedSignal(signal.Data.Direction.y >= 0 ? _speed : -_speed));
            }
        }

        CalculateSpeed(signal.Data.Direction);
    }

    private void CalculateSpeed(Vector2 input)
    {
        if (_states.States[PlayerState.Running])
            _speed = _config.RunningSpeed;
        else
            _speed = input.y >= 0 ? _config.WalkingSpeed : _config.WalkingBackSpeed;


        if (input == Vector2.zero)
        {
            _animator.SetInteger("Speed", 0);
            _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Running, false));
        }
        else
        {
            if (input.y >= 0)
            {
                _animator.SetInteger("Speed", _states.States[PlayerState.Running] ? 2 : 1);
            }
            else
            {
                _animator.SetInteger("Speed", -1);
            }
        }
    }

    private void EndRun()
    {
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Running, false));
    }

    private void StartRun()
    {
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Running, true));
    }

}
