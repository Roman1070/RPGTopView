using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public static class Extensions
{
    public static bool Mulitply(this IEnumerable<bool> array)
    {
        foreach(var value in array)
            if (!value) return false;
        
        return true;
    }

    public static bool Sum(this IEnumerable<bool> array)
    {
        foreach (var value in array)
            if (value) return true;

        return false;
    }

    public static bool Inverse(this bool b) => !b;

    public static void SetAlpha(this Graphic graphic, float alpha)
    {
        graphic.color = new UnityEngine.Color(graphic.color.r, graphic.color.g, graphic.color.b, alpha);
    }

    public static T Random<T>(this T[] arr) => arr[UnityEngine.Random.Range(0, arr.Length)];

    public static void SetLayerWeightSmooth(this Animator animator,MonoBehaviour context, string layer, bool toActive, float speed)
    {
        context.StartCoroutine(LayerWeightCoroutine(animator, layer, toActive, speed));
    }

    private static IEnumerator LayerWeightCoroutine(Animator animator, string layer, bool toActive, float speed)
    {
        if (toActive)
        {
            float w = 0;
            while (w < 1)
            {
                yield return new WaitForEndOfFrame();
                w += Time.deltaTime * speed;
                animator.SetLayerWeight(animator.GetLayerIndex(layer), w);
            }
        }
        else
        {
            float w = 1;
            while (w > 0)
            {
                yield return new WaitForEndOfFrame();
                w -= Time.deltaTime * speed;
                animator.SetLayerWeight(animator.GetLayerIndex(layer), w);
            }
        }
    }
}

