using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementService : LoadableService
{
    private Transform _cameraAnchor;
    private List<CameraMovementControllerBase> _controllers;
    private CameraMovementConfig _config;

    public CameraMovementService(SignalBus signalBus, PlayerView player, CameraMovementConfig config) : base(signalBus)
    {
        _cameraAnchor = player.Camera.transform.parent;
        _config = config;
        InitControllers();
    }

    private void InitControllers()
    {
        _controllers = new List<CameraMovementControllerBase>()
        {
            new CameraMovementGroundedController(_signalBus,_cameraAnchor,_config)
        };
    }
}
