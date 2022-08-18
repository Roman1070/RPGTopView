using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct InputDataPack
{
    public Vector2Int Direction;
    public Vector2 Rotation;
    public bool JumpAttempt;
    public bool SprintAttempt;
    public bool SprintBreak;
    public bool AttackAttempt;
}

public class OnInputDataRecievedSignal : ISignal
{
    public InputDataPack Data;

    public OnInputDataRecievedSignal(InputDataPack data)
    {
        Data = data;
    }

}
