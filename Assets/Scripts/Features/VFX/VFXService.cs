using System;
using System.Collections.Generic;

public enum VFXType
{
    LevelUp
}

public class VFXService : LoadableService
{
    private List<VFXControllerBase> _controllers;
    private PlayerView _player;

    public VFXService(SignalBus signalBus, PlayerView player) : base(signalBus)
    {
        _player = player;
    }

    public override void OnServicesLoaded(params LoadableService[] services)
    {
        InitControllers();
    }

    private void InitControllers()
    {
        _controllers = new List<VFXControllerBase>()
        {
            new PlayerVFXController(_signalBus,_player),
        };
    }
}
