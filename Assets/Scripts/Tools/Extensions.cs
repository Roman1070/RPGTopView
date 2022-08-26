using System.Collections.Generic;
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
}

