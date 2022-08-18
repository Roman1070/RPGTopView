using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModelRotationController : PlayerMovementControllerBase
{
    public PlayerModelRotationController(PlayerView player, SignalBus signalBus) : base(player, signalBus)
    {
        _signalBus.Subscribe<OnInputDataRecievedSignal>(OnInputRecieved, this);
    }

    private void OnInputRecieved(OnInputDataRecievedSignal signal)
    {
        float angle = signal.Data.Direction.y == 0 ? signal.Data.Direction.x*90 : signal.Data.Direction.x*45 * signal.Data.Direction.y;

        _player.Model.localEulerAngles = new Vector3(_player.Model.localEulerAngles.x,angle,_player.Model.localEulerAngles.z);
    }
}
