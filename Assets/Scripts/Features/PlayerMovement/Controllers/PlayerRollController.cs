using DG.Tweening;
using System;
using UnityEngine;

public class PlayerRollController : PlayerMovementControllerBase
{
    private float _stamina;
    private bool _rollAvailable;
    private Animator _animator;
    private PlayerMovementConfig _config;
    private float _z;
    private Vector3 _rollVector;

    public PlayerRollController(PlayerView player, SignalBus signalBus, PlayerMovementConfig config, UpdateProvider updateProvider, PlayerStatesService playerStatesService) : base(player, signalBus,playerStatesService)
    {
        _config = config;
        updateProvider.Updates.Add(Update);
        _signalBus.Subscribe<OnStaminaChangedSignal>(UpdateStamina, this);
        _signalBus.Subscribe<OnInputDataRecievedSignal>(CheckRollAttempt, this);
        _animator = _player.Model.GetComponent<Animator>();
    }

    private void CheckRollAttempt(OnInputDataRecievedSignal signal)
    {
        if (_rollAvailable && signal.Data.RollAttempt && _stamina >= _config.StaminaOnRoll)
        {
            Roll(signal.Data.Direction.y >= 0);
        }
    }

    private void Update()
    {
        _rollAvailable = !(!_states.States[PlayerState.Grounded] || _states.States[PlayerState.Rolling] || _states.States[PlayerState.Attacking]);

        if (Mathf.Abs(_z) <= 0.1f)
        {
            return;
        }

        /*if(_z > 0)
            _z -= Time.deltaTime * _config.RollDistance;
        else
            _z += Time.deltaTime * _config.RollDistance;

        _player.Controller.Move(_rollVector*_z * Time.deltaTime);*/

    }

    private void Roll(bool forward)
    {
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Rolling, true));
        _signalBus.FireSignal(new OnStaminaChangedSignal(_stamina - _config.StaminaOnRoll));
        _animator.SetTrigger(forward ? "Roll forward" : "Roll backward");

        _z = _config.RollDistance * (forward ? 1 : -1);
        _rollVector = _player.Model.transform.TransformDirection(new Vector3(0, 0, 1));
        _player.MoveAnim.SetCurve(_config.RollCurve);
        _player.MoveAnim.SetValues(_player.transform.position, _player.transform.position + _z * _rollVector);
        _player.MoveAnim.Play(0, () =>
        {
            _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Rolling, false));
        });



        DOVirtual.DelayedCall(0.8f, () =>
        {
        });
    }

    private void UpdateStamina(OnStaminaChangedSignal signal)
    {
        _stamina = signal.Stamina;
    }
}

public class PlayerAddForceController : PlayerMovementControllerBase
{
    public PlayerAddForceController(PlayerView player, SignalBus signalBus, PlayerStatesService playerStatesService) : base(player, signalBus, playerStatesService)
    {
    }
}
