using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovementController : PlayerMovementControllerBase
{
    private float _speed;
    private bool _isGrounded;
    private Vector3 _directionBeforeJump;
    private Vector3 _direction;
    private float _stamina;
    private bool _isRunning;
    private PlayerMovementConfig _config;
    private bool _staminaIsGrowing;

    public PlayerMovementController(PlayerView player, SignalBus signalBus, UpdateProvider updateProvider, PlayerMovementConfig config) : base(player,signalBus)
    {
        _config = config;

        _signalBus.Subscribe<OnInputDataRecievedSignal>(OnInputRecieved, this);
        _signalBus.Subscribe<OnGroundedStatusChangedSignal>(UpdateGroundedStatus, this);
        updateProvider.Updates.Add(UpdateStamina);

        Cursor.lockState = CursorLockMode.Locked;
        _stamina = _config.MaxStamina;
    }

    private void UpdateGroundedStatus(OnGroundedStatusChangedSignal signal)
    {
        _isGrounded = signal.IsGrounded;
        _directionBeforeJump = _direction;
    }

    private void OnInputRecieved(OnInputDataRecievedSignal signal)
    {
        var moveDirection = new Vector3(signal.Direction.x, 0, signal.Direction.y);
        moveDirection = _player.transform.TransformDirection(moveDirection);

        if(!_isRunning && signal.SprintAttempt)
        {
            StartRun();
        }
        
        if (_isRunning && signal.SprintBreak)
        {
            EndRun();
        }

        if (_isRunning)
            _speed = _config.RunningSpeed;
        else
            _speed = signal.Direction.y >= 0 ? _config.WalkingSpeed : _config.WalkingBackSpeed;

        _direction = _isGrounded ? moveDirection : _directionBeforeJump;

        _player.Controller.Move(_direction * _speed * Time.deltaTime);
        _player.transform.Rotate(Vector3.up, signal.Rotation.x);
    }

    private void UpdateStamina()
    {
        if (_isRunning)
        {
            _stamina -= _config.StaminaRegeneration * Time.deltaTime;

            if (_stamina <= 0) EndRun();
        }
        else if (_staminaIsGrowing)
        { 
            _stamina += _config.StaminaConsuming * Time.deltaTime; 
        }
        _signalBus.FireSignal(new OnStaminaChangedSignal(_stamina));
    }

    private void EndRun()
    {
        _isRunning = false;
        _player.StartCoroutine(AllowRegeratingStamina(_config.StaminaRegeneratingDelay));
    }

    private void StartRun()
    {
        _isRunning = true;
        _staminaIsGrowing = false;
        _player.StopCoroutine(AllowRegeratingStamina(_config.StaminaRegeneratingDelay));
    }

    private IEnumerator AllowRegeratingStamina(float delay)
    {
        yield return new WaitForSeconds(delay);
        _staminaIsGrowing = true;
    }
}
