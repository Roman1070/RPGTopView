using UnityEngine;

public class CameraMovementControllerBase
{
    protected SignalBus _signalBus;
    protected Transform _cameraAnchor;
    protected CameraMovementConfig _config;

    public CameraMovementControllerBase(SignalBus signalBus, Transform cameraAnchor, CameraMovementConfig config)
    {
        _signalBus = signalBus;
        _cameraAnchor = cameraAnchor;
        _config = config;
    }
}