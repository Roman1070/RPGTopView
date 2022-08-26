using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OpacityAnimation : BaseProgrammableAnimationMono<float>
{
    public Graphic Graphic;

    public override void Process(float t)
    {
        Graphic.SetAlpha(StartValue + (EndValue - StartValue) * _Kurwa.Evaluate(t));
    }

}
