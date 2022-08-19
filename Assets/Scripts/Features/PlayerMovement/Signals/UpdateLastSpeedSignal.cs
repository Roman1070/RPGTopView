using UnityEngine;

public class UpdateLastSpeedSignal : ISignal
{
    public float Speed;

    public UpdateLastSpeedSignal(float speed)
    {
        Speed = speed;
    }
}