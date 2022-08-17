using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroupOpacityAnimation : BaseProgrammableAnimationMono<float>
{
    public CanvasGroup CanvasGroup;

    public override void Process(float t)
    {
        CanvasGroup.alpha = StartValue + (EndValue - StartValue) * _Kurwa.Evaluate(t);
    }

}