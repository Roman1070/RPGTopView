using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class PlayerMovementController : PlayerMovementControllerBase
{
    private float _speed;
    private float _stamina;
    private bool MovementAvailable => new bool[]
    {
        !_states.States[PlayerState.Grounded],
        _states.States[PlayerState.Rolling],
        _states.States[PlayerState.Interacting],
        _states.States[PlayerState.Attacking]
    }.Sum().Inverse();


    private PlayerMovementConfig _config;
    private Animator _animator;
    private bool _forceSprint;
    private float _lastSpeed;

    public PlayerMovementController(PlayerView player, SignalBus signalBus, UpdateProvider updateProvider, PlayerMovementConfig config, PlayerStatesService statesService) : base(player, signalBus, statesService)
    {
        _config = config;
        _animator = _player.Model.GetComponent<Animator>();

        _signalBus.Subscribe<OnInputDataRecievedSignal>(OnInputRecieved, this);
        _signalBus.Subscribe<OnStaminaChangedSignal>(OnStaminaChanged, this);
        _signalBus.Subscribe<OnPlayerStateChangedSignal>(OnStateChanged, this);

        Cursor.lockState = CursorLockMode.Locked;
        _stamina = _config.MaxStamina;
    }

    private void OnStateChanged(OnPlayerStateChangedSignal obj)
    {
        _forceSprint = (obj.State == PlayerState.Grounded && obj.Value ||
            obj.State == PlayerState.Rolling && !obj.Value || obj.State == PlayerState.Attacking && !obj.Value) && _lastSpeed== _config.RunningSpeed;
    }

    private void OnStaminaChanged(OnStaminaChangedSignal signal)
    {
        _stamina = signal.Stamina;
        if (_stamina <= 0) EndRun();
    }

    private void OnInputRecieved(OnInputDataRecievedSignal signal)
    {
        if (MovementAvailable)
        {
            var moveDirection = new Vector3(signal.Data.Direction.x, 0, signal.Data.Direction.y);
            moveDirection = _player.transform.TransformDirection(moveDirection);

            if ((signal.Data.SprintAttempt|| _forceSprint) && signal.Data.Direction.y >= 0 && MovementAvailable && _stamina > 10)
                StartRun();

            if (signal.Data.Direction.y < 0 || signal.Data.Direction == Vector2.zero)
                EndRun();

            _player.Controller.Move(moveDirection * _speed * Time.deltaTime);
            if (signal.Data.Direction == Vector2.zero)
            {
                _lastSpeed = 0;
            }
            else
            {
                _lastSpeed = signal.Data.Direction.y >= 0 ? _speed : -_speed;
            }
            _signalBus.FireSignal(new UpdateLastSpeedSignal(_lastSpeed));
            CalculateSpeed(signal.Data.Direction);
        }
        else
        {
            CalculateSpeed(Vector2Int.zero);
        }

        CalculateBlend(signal.Data.Direction);

        if (signal.Data.SprintBreak) EndRun();

        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Idle, signal.Data.Direction == Vector2Int.zero && MovementAvailable));

        _forceSprint = false;
    }

    private void CalculateBlend(Vector2 input)
    {
        _animator.SetFloat("BlendSpeed", 0);
        if (input != Vector2.zero)
        {
            if (input.y >= 0)
            {
                _animator.SetFloat("BlendSpeed", _states.States[PlayerState.Running] ? 2 : 1);
            }
            else
            {
                _animator.SetFloat("BlendSpeed", -1);
            }
        }
    }

    private void CalculateSpeed(Vector2 input)
    {
        if (_states.States[PlayerState.Running])
            _speed = _config.RunningSpeed;
        else
            _speed = input.y >= 0 ? _config.WalkingSpeed : _config.WalkingBackSpeed;


        if (input == Vector2.zero)
        {
            _animator.SetFloat("Speed", 0, 0.1f, Time.deltaTime);
            _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Running, false));
        }
        else
        {
            if (input.y >= 0)
            {
                _animator.SetBool("IsArmed", _states.States[PlayerState.IsArmed] || _states.States[PlayerState.DrawingWeapon]);
                _animator.SetFloat("Speed", _states.States[PlayerState.Running] ? 2 : 1, 0.1f, Time.deltaTime);
            }
            else
            {
                _animator.SetFloat("Speed", -1, 0.15f, Time.deltaTime);
            }
        }
    }

    private void EndRun()
    {
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Running, false));
        _lastSpeed = 0;
    }

    private void StartRun()
    {
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Running, true));
    }

}
