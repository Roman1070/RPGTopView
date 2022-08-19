using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelRotationController : PlayerMovementControllerBase
{
    private bool _rotationAvailable;
    public PlayerModelRotationController(PlayerView player, SignalBus signalBus) : base(player, signalBus)
    {
        _signalBus.Subscribe<OnInputDataRecievedSignal>(OnInputRecieved, this);
        _signalBus.Subscribe<SendCharacterStatesSignal>(GetCharacterStates, this);
    }

    private void GetCharacterStates(SendCharacterStatesSignal signal)
    {
        _rotationAvailable = !(signal.States[PlayerState.Rolling] || signal.States[PlayerState.Jumping] || signal.States[PlayerState.Collecting]);
    }

    private void OnInputRecieved(OnInputDataRecievedSignal signal)
    {
        if (!_rotationAvailable) return;

        float angle = signal.Data.Direction.y == 0 ? signal.Data.Direction.x*90 : signal.Data.Direction.x*45 * signal.Data.Direction.y;

        _player.Model.localEulerAngles = new Vector3(_player.Model.localEulerAngles.x,angle,_player.Model.localEulerAngles.z);
    }
}
