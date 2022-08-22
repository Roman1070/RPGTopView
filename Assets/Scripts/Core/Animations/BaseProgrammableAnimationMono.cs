using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseProgrammableAnimationMono<T> : MonoBehaviour
{
    [SerializeField]
    protected AnimationCurve _Kurwa;
    [SerializeField]
    private T _StartValue;
    [SerializeField]
    private T _EndValue;
    [SerializeField]
    private float _Duration;

    private bool _Reversed;
    private ProgrammableAnimation _Animation;

    public float Duration => _Duration;
    public T StartValue
    {
        get
        {
            return _Reversed ? _EndValue : _StartValue;
        }
    }
    public T EndValue
    {
        get
        {
            return _Reversed ? _StartValue : _EndValue;
        }
    }

    private void Awake()
    {
        _Animation = new ProgrammableAnimation(this);
    }

    public void SetValues(T startVal, T finalVal)
    {
        _StartValue = startVal;
        _EndValue = finalVal;
    }

    public void SetCurve(AnimationCurve curve)
    {
        _Kurwa = curve;
    }

    public void SetDuration(float value)
    {
        _Duration = value;
    }

    public void Play(float delay = 0,Action callback=null, bool reverse=false)
    {
        _Reversed = reverse;
        StopAllCoroutines();
        if (_Animation == null)
            _Animation = new ProgrammableAnimation(this);

        _Animation.Play(_Duration, Process, callback,delay);
    }

    public void SetProgress(float value)
    {
        if(_Reversed)
            _Animation.SetProgress(1-value, Process);
        else
            _Animation.SetProgress(value, Process);
    }

    public abstract void Process(float t);
}
