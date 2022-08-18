using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class PlayerMovementController : PlayerMovementControllerBase
{
    private float _speed;
    private float _stamina;
    private bool _isGrounded;
    private bool _isRunning;
    private bool _staminaIsGrowing;
    private Vector3 _directionBeforeJump;
    private Vector3 _direction;
    private PlayerMovementConfig _config;
    private Animator _animator;

    private Tween _StaminaGrowthEnabler;

    public PlayerMovementController(PlayerView player, SignalBus signalBus, UpdateProvider updateProvider, PlayerMovementConfig config) : base(player,signalBus)
    {
        _config = config;
        _animator = _player.Model.GetComponent<Animator>();

        _signalBus.Subscribe<OnInputDataRecievedSignal>(OnInputRecieved, this);
        _signalBus.Subscribe<OnGroundedStatusChangedSignal>(UpdateGroundedStatus, this);
        _signalBus.Subscribe<OnStaminaChangedSignal>(OnStaminaChanged, this);
        updateProvider.Updates.Add(UpdateStamina);

        Cursor.lockState = CursorLockMode.Locked;
        _stamina = _config.MaxStamina;
    }

    private void OnStaminaChanged(OnStaminaChangedSignal signal)
    {
        if (signal.Stamina < _stamina)
        {
            _StaminaGrowthEnabler.Kill();
            _staminaIsGrowing = false;
            _StaminaGrowthEnabler = DOVirtual.DelayedCall(_config.StaminaRegeneratingDelay, () =>
            {
                _staminaIsGrowing = true;
            });
        }
        _stamina = signal.Stamina;
    }

    private void UpdateGroundedStatus(OnGroundedStatusChangedSignal signal)
    {
        _isGrounded = signal.IsGrounded;
        _directionBeforeJump = _direction;
    }

    private void OnInputRecieved(OnInputDataRecievedSignal signal)
    {
        var moveDirection = new Vector3(signal.Data.Direction.x, 0, signal.Data.Direction.y);
        moveDirection = _player.transform.TransformDirection(moveDirection);

        if(!_isRunning && signal.Data.SprintAttempt &&_isGrounded)
            StartRun();
        
        if (_isRunning && signal.Data.SprintBreak)
            EndRun();

        _direction = _isGrounded ? moveDirection : _directionBeforeJump;

        CalculateSpeed(signal.Data.Direction);

        _player.Controller.Move(_direction * _speed * Time.deltaTime);
        _player.transform.Rotate(Vector3.up, signal.Data.Rotation.x);
    }

    private void CalculateSpeed(Vector2 input)
    {
        if (_isRunning)
            _speed = _config.RunningSpeed;
        else
            _speed = input.y >= 0 ? _config.WalkingSpeed : _config.WalkingBackSpeed;


        if (input == Vector2.zero)
            _animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
        else _animator.SetFloat("Speed", _isRunning ? 1 : 0.5f, 0.1f, Time.deltaTime);
    }

    private void UpdateStamina()
    {
        if (_isRunning)
        {
            _stamina -= _config.StaminaConsuming * Time.deltaTime;

            if (_stamina <= 0) EndRun();
        }
        else if (_staminaIsGrowing)
        { 
            _stamina += _config.StaminaRegeneration * Time.deltaTime;
            _stamina = Mathf.Clamp(_stamina, 0, _config.MaxStamina);
        }

        _signalBus.FireSignal(new OnStaminaChangedSignal(_stamina));
    }

    private void EndRun()
    {
        _isRunning = false;
        _staminaIsGrowing = false;
        _StaminaGrowthEnabler = DOVirtual.DelayedCall(_config.StaminaRegeneratingDelay, () =>
        {
            _staminaIsGrowing = true;
        });
    }

    private void StartRun()
    {
        _isRunning = true;
        _staminaIsGrowing = false;
        _StaminaGrowthEnabler.Kill();
    }

}
