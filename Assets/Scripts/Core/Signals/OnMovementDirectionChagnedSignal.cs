using UnityEngine;

public class OnMovementDirectionChagnedSignal : ISignal
{
    public Vector2Int Direction;

    public OnMovementDirectionChagnedSignal(Vector2Int direction)
    {
        Direction = direction;
    }
}
