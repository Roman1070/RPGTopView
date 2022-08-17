using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PositionAnimation : BaseProgrammableAnimationMono<Vector3>
{
    public override void Process(float t)
    {
        transform.position = StartValue + (EndValue - StartValue) * _Kurwa.Evaluate(t);
    }
}
