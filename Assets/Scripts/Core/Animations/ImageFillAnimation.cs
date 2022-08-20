using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageFillAnimation : BaseProgrammableAnimationMono<float>
{
    [SerializeField]
    private Image _Image;

    public Image Image => _Image;

    public override void Process(float t)
    {
        _Image.fillAmount = StartValue + (EndValue - StartValue) * _Kurwa.Evaluate(t);
    }
}
