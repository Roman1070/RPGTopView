using DG.Tweening;
using UnityEngine;

public class PlayerRotationController : PlayerMovementControllerBase
{
    private Tween _rotateTween;
    private MainCameraAnchor _cameraAnchor;
    private Vector3 _previousCameraRotation;
    private Vector3 TargetRotation => new Vector3(_player.transform.eulerAngles.x, _cameraAnchor.transform.eulerAngles.y, _player.transform.eulerAngles.z);

    public PlayerRotationController(PlayerView player, SignalBus signalBus, PlayerStatesService statesService, MainCameraAnchor cameraAnchor) : base(player, signalBus, statesService)
    {
        _cameraAnchor = cameraAnchor;
        _signalBus.Subscribe<OnInputDataRecievedSignal>(OnInputRecieved, this);
        _signalBus.Subscribe<OnPlayerStateChangedSignal>(OnStateChanged, this);
    }

    private void OnInputRecieved(OnInputDataRecievedSignal signal)
    {
        if (signal.Data.Direction != Vector2Int.zero && !(_states.States[PlayerState.Interacting] || _states.States[PlayerState.Rolling])) RotatePlayer();
        if (TargetRotation != _previousCameraRotation)
        {
            _previousCameraRotation = TargetRotation;
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
