using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovementService : LoadableService
{
    private Transform _cameraAnchor;
    private List<CameraMovementControllerBase> _controllers;
    private CameraMovementConfig _config;
    private PlayerView _player;
    private UpdateProvider _updateProvider;

    public CameraMovementService(SignalBus signalBus, PlayerView player, CameraMovementConfig config, UpdateProvider updateProvider,MainCameraAnchor camera) : base(signalBus)
    {
        _player = player;
        _cameraAnchor = camera.transform;
        _config = config;
        _updateProvider = updateProvider;
        InitControllers();
    }

    public override void OnServicesLoaded(params LoadableService[] services)
    {
    }

    private void InitControllers()
    {
        _controllers = new List<CameraMovementControllerBase>()
        {
            new CameraMovementGroundedController(_signalBus,_cameraAnchor,_config,_player,_updateProvider)
        };
    }
}
