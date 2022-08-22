using System;
using UnityEngine;

public class CameraMovementGroundedController : CameraMovementControllerBase
{
    private PlayerView _player;
    private UpdateProvider _updateProvider;

    public CameraMovementGroundedController(SignalBus signalBus, Transform cameraAnchor, CameraMovementConfig config, PlayerView player, UpdateProvider updateProvider) : base(signalBus, cameraAnchor, config)
    {
        _player = player;
        _updateProvider = updateProvider;
        signalBus.Subscribe<OnInputDataRecievedSignal>(GetInput, this);
        _updateProvider.Updates.Add(Update);
    }

    private void Update()
    {
        _cameraAnchor.position = _player.transform.position + _config.CameraOffset;
    }

    private void GetInput(OnInputDataRecievedSignal signal)
    {
        float angleX = signal.Data.Rotation.y * (_config.InvertY ? -1 : 1);
        float angleY = signal.Data.Rotation.x * (_config.InvertX ? -1 : 1);
        float XAfterRotation = _cameraAnchor.transform.localEulerAngles.x + angleX;
        if (XAfterRotation > 300) XAfterRotation -= 360;

        if(XAfterRotation>_config.MinAngle && XAfterRotation<_config.MaxAngle) _cameraAnchor.Rotate(Vector3.right, angleX);
        _cameraAnchor.Rotate(Vector3.up,angleY);
        _cameraAnchor.localEulerAngles = new Vector3(_cameraAnchor.localEulerAngles.x, _cameraAnchor.localEulerAngles.y, 0);
    }
}
