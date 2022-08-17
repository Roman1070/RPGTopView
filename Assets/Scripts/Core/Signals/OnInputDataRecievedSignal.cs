using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnInputDataRecievedSignal : ISignal
{
    public Vector2Int Direction;
    public Vector2 Rotation;
    public bool Jump;
    public bool SprintAttempt;
    public bool SprintBreak;

    public OnInputDataRecievedSignal(Vector2Int direction)
    {
        Direction = direction;
    }

    public OnInputDataRecievedSignal(Vector2Int direction, bool jump)
    {
        Direction = direction;
        Jump = jump;
    }

    public OnInputDataRecievedSignal(Vector2Int direction, Vector2 rotation, bool jump, bool sprint, bool sprintBreak)
    {
        Direction = direction;
        Rotation = rotation;
        Jump = jump;
        SprintAttempt = sprint;
        SprintBreak = sprintBreak;
    }
}
