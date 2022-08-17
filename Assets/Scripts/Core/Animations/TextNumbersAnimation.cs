using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TextNumbersAnimation : BaseProgrammableAnimationMono<float>
{
    [SerializeField] private Text _Text;
    [SerializeField] private int _Digits;

    public override void Process(float t)
    {
        _Text.text = Math.Round(StartValue + (EndValue - StartValue) * _Kurwa.Evaluate(t),_Digits).ToString();    
    }
}
