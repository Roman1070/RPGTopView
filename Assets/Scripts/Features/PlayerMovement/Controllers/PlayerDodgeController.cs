using UnityEngine;

public class PlayerDodgeController : PlayerMovementControllerBase
{
    private bool _dodgeAvailable;
    private Animator _animator;
    private PlayerMovementConfig _config;
    private float _x;
    private Vector3 _rollVector;

    public PlayerDodgeController(PlayerView player, SignalBus signalBus, PlayerStatesService playerStatesService, UpdateProvider updateProvider, PlayerMovementConfig config) : base(player, signalBus, playerStatesService)
    {
        _animator = _player.Model.GetComponent<Animator>();
        _config = config;
        updateProvider.Updates.Add(Update);
        _signalBus.Subscribe<OnInputDataRecievedSignal>(CheckDodgeAttempt, this);
    }
    private void CheckDodgeAttempt(OnInputDataRecievedSignal signal)
    {
        if (_dodgeAvailable && signal.Data.DodgeAttempt)
        {
            Dodge(signal.Data.Direction.x > 0);
        }
    }

    private void Update()
    {
        _dodgeAvailable = !(!_states.States[PlayerState.Grounded] || _states.States[PlayerState.Rolling] || _states.States[PlayerState.Dodging] || _states.States[PlayerState.Attacking]);
    }

    private void Dodge(bool right)
    {
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Dodging, true));
        _animator.SetTrigger(right ? "Dodge right" : "Dodge left");

        _x = _config.RollDistance * (right ? 1 : -1);
        _rollVector = _player.Model.transform.TransformDirection(new Vector3(1, 0, 0));
        _player.MoveAnim.SetCurve(_config.DodgeCurve);
        _player.MoveAnim.SetValues(_player.transform.position, _player.transform.position + _x * _rollVector);
        _player.MoveAnim.Play(0, () =>
        {
            _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Dodging, false));
        });
    }
}
