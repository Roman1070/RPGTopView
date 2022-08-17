using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ColorAnimation : BaseProgrammableAnimationMono<Color>
{
    public  Graphic Graphic;
    public override void Process(float t)
    {
        Graphic.color = StartValue + (EndValue - StartValue) * _Kurwa.Evaluate(t);
    }
}
