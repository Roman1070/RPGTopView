using UnityEngine;

public class UpdateVelocityBeforeJumpSignal : ISignal
{
    public Vector3 Velocity;

    public UpdateVelocityBeforeJumpSignal(Vector3 velocity)
    {
        Velocity = velocity;
    }
}