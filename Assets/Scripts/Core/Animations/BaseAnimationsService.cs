using UnityEngine;


public static class AnimationsExtensions
{
    public static void RunAnimations(float delay, bool reverse = false, params BaseProgrammableAnimationMono<float>[] animations)
    {
        RunAnimations<float>(delay, reverse, animations);
    }
    public static void RunAnimations(float delay, bool reverse = false, params BaseProgrammableAnimationMono<Vector3>[] animations)
    {
        RunAnimations<Vector3>(delay, reverse, animations);
    }
    public static void RunAnimations(float delay, bool reverse = false, params BaseProgrammableAnimationMono<Transform>[] animations)
    {
        RunAnimations<Transform>(delay, reverse, animations);
    }
    public static void RunAnimations(float delay, bool reverse = false, params BaseProgrammableAnimationMono<Color>[] animations)
    {
        RunAnimations<Color>(delay, reverse, animations);
    }

    public static void RunAnimations<T>(float delay, bool reverse = false, params BaseProgrammableAnimationMono<T>[] animations)
    {
        foreach (var animation in animations)
        {
            if (animation)
            {
                animation.StopAllCoroutines();
                animation.Play(delay, null, reverse);
            }
        }
    }

    public static void Reset(this OpacityAnimation component, float targetValue = 0)
    {
        component.SetProgress(targetValue);
    }

    public static void Reset(this GroupOpacityAnimation component, float targetValue = 0)
    {
        component.SetProgress(targetValue);
    }

    public static void Reset(this ColorAnimation component, float targetValue = 0)
    {
        component.SetProgress(targetValue);
    }

    public static void Reset(this TransformAnimation component, float targetValue = 0)
    {
        component.SetProgress(targetValue);
    }

    public static void Reset(this ScaleAnimation component, float targetValue = 0)
    {
        component.SetProgress(targetValue);
    }
    public static void SetAnimationsProgress(float value, params BaseProgrammableAnimationMono<Color>[] animations)
    {
        SetProgress(value, animations);
    }
    public static void SetAnimationsProgress(float value, params BaseProgrammableAnimationMono<Transform>[] animations)
    {
        SetProgress(value, animations);
    }
    public static void SetAnimationsProgress(float value, params BaseProgrammableAnimationMono<Vector3>[] animations)
    {
        SetProgress(value, animations);
    }
    public static void SetAnimationsProgress(float value, params BaseProgrammableAnimationMono<float>[] animations)
    {
        SetProgress(value, animations);
    }

    private static void SetProgress<T>(float value, params BaseProgrammableAnimationMono<T>[] animations)
    {
        foreach (var animation in animations)
        {
            if (animation)
            {
                animation.SetProgress(value);
            }
        }
    }
}

