using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelUpdateService : LoadableService
{
    public PlayerModelUpdateService(SignalBus signalBus) :base(signalBus)
    {

    }
    public override void OnServicesLoaded(params LoadableService[] services)
    {
        throw new System.NotImplementedException();
    }
}
