using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationAnimation : BaseProgrammableAnimationMono<Vector3>
{
    public override void Process(float t)
    {
        transform.localEulerAngles = StartValue + (EndValue - StartValue) * _Kurwa.Evaluate(t);
    }
}
