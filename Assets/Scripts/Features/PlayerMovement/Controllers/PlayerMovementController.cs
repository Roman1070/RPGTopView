using DG.Tweening;
using System;
using UnityEngine;

public class PlayerMovementController : PlayerMovementControllerBase
{
    private float _speed;
    private float _stamina;
    private bool _movementAvailable;
    private PlayerMovementConfig _config;
    private Animator _animator;
    private Tween _rotateTween;
    private bool _playerRotationAllowed;
    private Vector3 _previousCameraRotation;

    private Vector3 TargetRotation => new Vector3(_player.transform.eulerAngles.x, _player.Camera.transform.parent.eulerAngles.y, _player.transform.eulerAngles.z);

    public PlayerMovementController(PlayerView player, SignalBus signalBus, UpdateProvider updateProvider, PlayerMovementConfig config, PlayerStatesService statesService) : base(player, signalBus, statesService)
    {
        _config = config;
        _animator = _player.Model.GetComponent<Animator>();

        _signalBus.Subscribe<OnPlayerStateChangedSignal>(OnStateChanged, this);
        _signalBus.Subscribe<OnInputDataRecievedSignal>(OnInputRecieved, this);
        _signalBus.Subscribe<OnStaminaChangedSignal>(OnStaminaChanged, this);

        Cursor.lockState = CursorLockMode.Locked;
        _stamina = _config.MaxStamina;
    }

    private void OnStateChanged(OnPlayerStateChangedSignal obj)
    {
        if (obj.State == PlayerState.Idle || obj.State == PlayerState.Idle)
        {
            if (!obj.Value)
            {
                RotatePlayer();
            }
        }
    }

    private void OnStaminaChanged(OnStaminaChangedSignal signal)
    {
        _stamina = signal.Stamina;
        if (_stamina <= 0) EndRun();
    }

    private void OnInputRecieved(OnInputDataRecievedSignal signal)
    {
        if (TargetRotation != _previousCameraRotation)
        {
            _previousCameraRotation = TargetRotation;
            if(signal.Data.Direction!=Vector2Int.zero && !_states.States[PlayerState.Interacting]) RotatePlayer();
        }

        _movementAvailable = !(!_states.States[PlayerState.Grounded] || _states.States[PlayerState.Rolling]
         || _states.States[PlayerState.Interacting] || _states.States[PlayerState.Attacking]);


        if (_movementAvailable)
        {
            var moveDirection = new Vector3(signal.Data.Direction.x, 0, signal.Data.Direction.y);
            moveDirection = _player.transform.TransformDirection(moveDirection);

            if (!_states.States[PlayerState.Running] && signal.Data.SprintAttempt && signal.Data.Direction.y >= 0 && _movementAvailable && _stamina > 10)
                StartRun();

            if (_states.States[PlayerState.Running] && (signal.Data.SprintBreak || signal.Data.Direction.y < 0 || signal.Data.Direction == Vector2.zero))
                EndRun();

            _player.Controller.Move(moveDirection * _speed * Time.deltaTime);
            if (signal.Data.Direction == Vector2.zero)
            {
                _signalBus.FireSignal(new UpdateLastSpeedSignal(0));
            }
            else
            {
                _signalBus.FireSignal(new UpdateLastSpeedSignal(signal.Data.Direction.y >= 0 ? _speed : -_speed));
            }
        }
        else
        {
            EndRun();
        }
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Idle, signal.Data.Direction == Vector2Int.zero && _movementAvailable));

        CalculateSpeed(signal.Data.Direction);
    }

    private void RotatePlayer()
    {
        _rotateTween.Kill();
        _rotateTween = null;
        _rotateTween = _player.transform.DORotate(TargetRotation, 0.2f).OnComplete(() => _playerRotationAllowed = true);
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
