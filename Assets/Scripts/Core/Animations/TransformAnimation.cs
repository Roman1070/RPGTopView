using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransformAnimation : BaseProgrammableAnimationMono<Transform>
{
    public override void Process(float t)
    {
        transform.position = StartValue.position + (EndValue.position - StartValue.position) * _Kurwa.Evaluate(t);
        transform.localEulerAngles = StartValue.eulerAngles + (EndValue.eulerAngles - StartValue.eulerAngles) * _Kurwa.Evaluate(t);
        transform.localScale = StartValue.localScale + (EndValue.localScale - StartValue.localScale) * _Kurwa.Evaluate(t);
    }
}
