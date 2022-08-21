using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelRotationController : PlayerMovementControllerBase
{
    private bool _rotationAvailable;
    private RotationAnimation _anim;
    private Vector2Int _targetDirection;

    public PlayerModelRotationController(PlayerView player, SignalBus signalBus, PlayerStatesService playerStatesService) : base(player, signalBus, playerStatesService)
    {
        _anim = player.Model.GetComponent<RotationAnimation>();
        _signalBus.Subscribe<OnMovementDirectionChagnedSignal>(OnDirectionChanged, this);
        _signalBus.Subscribe<OnInputDataRecievedSignal>(OnInput, this);
        _signalBus.Subscribe<OnPlayerStateChangedSignal>(OnStateChange, this);
    }

    private void OnInput(OnInputDataRecievedSignal obj)
    {
        _rotationAvailable = !(_states.States[PlayerState.Rolling] || _states.States[PlayerState.Interacting]);
        _targetDirection = obj.Data.Direction;
    }

    private void OnStateChange(OnPlayerStateChangedSignal obj)
    {
        if ((obj.State == PlayerState.Rolling || obj.State == PlayerState.Interacting) && obj.Value == false)
        {
            _rotationAvailable = true;
            OnDirectionChanged(new OnMovementDirectionChagnedSignal(_targetDirection));
        }
    }

    private void OnDirectionChanged(OnMovementDirectionChagnedSignal signal)
    {
        if (!_rotationAvailable) return;

        float angle = signal.Direction.y == 0 ? signal.Direction.x * 90 : signal.Direction.x * 45 * signal.Direction.y;

        Vector3 startValue = _player.Model.localEulerAngles;
        if (startValue.y > 180)
            startValue = new Vector3(startValue.x, startValue.y-360,startValue.z);

        _anim.SetValues(startValue, new Vector3(_player.Model.localEulerAngles.x, angle, _player.Model.localEulerAngles.z));
        _anim.Play();
    }
}
