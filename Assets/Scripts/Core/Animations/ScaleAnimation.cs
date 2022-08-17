using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScaleAnimation : BaseProgrammableAnimationMono<Vector3>
{
    public override void Process(float t)
    {
        transform.localScale = StartValue + (EndValue - StartValue) * _Kurwa.Evaluate(t);
    }
}
