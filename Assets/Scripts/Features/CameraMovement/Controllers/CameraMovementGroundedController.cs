using UnityEngine;

public class CameraMovementGroundedController : CameraMovementControllerBase
{
    public CameraMovementGroundedController(SignalBus signalBus, Transform cameraAnchor, CameraMovementConfig config) : base(signalBus, cameraAnchor,config)
    {
        signalBus.Subscribe<OnInputDataRecievedSignal>(GetInput,this);
    }

    private void GetInput(OnInputDataRecievedSignal signal)
    {
        float deltaX = signal.Data.Rotation.y * (_config.InvertY ? -1 : 1);
        float XAfterRotation = _cameraAnchor.transform.localEulerAngles.x + deltaX;
        if (XAfterRotation > 300) XAfterRotation -= 360;
        if(XAfterRotation>_config.MinAngle && XAfterRotation<_config.MaxAngle) _cameraAnchor.Rotate(Vector3.right, deltaX);
    }
}
