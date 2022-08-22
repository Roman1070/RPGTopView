using DG.Tweening;
using UnityEngine;

public class PlayerRotationController : PlayerMovementControllerBase
{
    private Tween _rotateTween;
    private Vector3 _previousCameraRotation;
    private Vector3 TargetRotation => new Vector3(_player.transform.eulerAngles.x, _player.Camera.transform.parent.eulerAngles.y, _player.transform.eulerAngles.z);

    public PlayerRotationController(PlayerView player, SignalBus signalBus, PlayerStatesService statesService) : base(player, signalBus, statesService)
    {
        _signalBus.Subscribe<OnInputDataRecievedSignal>(OnInputRecieved, this);
        _signalBus.Subscribe<OnPlayerStateChangedSignal>(OnStateChanged, this);
    }

    private void OnInputRecieved(OnInputDataRecievedSignal signal)
    {
        if (TargetRotation != _previousCameraRotation)
        {
            _previousCameraRotation = TargetRotation;
            if (signal.Data.Direction != Vector2Int.zero && !(_states.States[PlayerState.Interacting] || _states.States[PlayerState.Rolling])) RotatePlayer();
        }
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

    private void RotatePlayer()
    {
        _rotateTween.Kill();
        _rotateTween = null;
        _rotateTween = _player.transform.DORotate(TargetRotation, 0.2f);
    }
}
