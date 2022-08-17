using UnityEngine;
using UnityEngine.UI;

public class SliderAnimation : BaseProgrammableAnimationMono<float>
{
    [SerializeField] private Slider _Slider;

    public Slider Slider =>_Slider;
    public override void Process(float t)
    {
        _Slider.value = StartValue + (EndValue - StartValue) * _Kurwa.Evaluate(t);
    }
}

