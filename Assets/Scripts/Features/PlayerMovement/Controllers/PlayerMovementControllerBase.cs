using System;
using System.Collections;
using System.Collections.Generic;

public class PlayerMovementControllerBase
{
    protected SignalBus _signalBus;
    protected PlayerView _player;


    public PlayerMovementControllerBase(PlayerView player, SignalBus signalBus)
    {
        _player = player;
        _signalBus = signalBus;
    }

}
