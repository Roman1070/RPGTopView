using System;
using System.Collections;
using UnityEngine;

public class ProgrammableAnimation
{
    private Coroutine _LastAnimation;
    private MonoBehaviour _Context;

    public ProgrammableAnimation(MonoBehaviour context)
    {
        _Context = context;
    }

    public void Play(float duration, Action<float> action, Action callback,float delay = 0)
    {
        Stop();
        _LastAnimation = _Context.StartCoroutine(CreateAnimation(duration, action, callback, delay));
    }

    public void Stop()
    {
        if (_LastAnimation != null)
            _Context.StopCoroutine(_LastAnimation);
    }

    private IEnumerator CreateAnimation(float duration, Action<float> action, Action callback,float delay = 0)
    {
        yield return new WaitForSeconds(delay);
        float expiredSeconds = 0;
        float progress = 0;

        while (progress < 1)
        {
            expiredSeconds += Time.deltaTime;
            progress = expiredSeconds / duration;
            action.Invoke(progress);
            yield return null;
        }

        callback?.Invoke();
    }

    public void SetProgress(float value,Action<float> action)
    {
        Stop();
        action.Invoke(value);
    }
}