using System;
using UnityEngine;

[Serializable]
public class PalleteColor
{
    public string Key;
    private bool _Dirty = true;

    public Color Color
    {
        get
        {
            RefreshColor();
            return _Color;
        }
    }
    private Color _Color;

    public PalleteColor() { }

    public PalleteColor(string key)
    {
        Key = key;
    }

    public void MarkDirty()
    {
        _Dirty = true;
    }

    private void RefreshColor()
    {
        if (_Dirty)
        {
            if (Application.isPlaying)
            {
                _Color = PalleteColorConfig.Instance.GetRawColor(Key);
            }
            else
            {
#if UNITY_EDITOR
                _Color = PalleteColorConfig.Instance.ExtractRawColor(Key);
#endif
            }
        }
        _Dirty = false;
    }

    public static implicit operator Color(PalleteColor c)
    {
        if (c == null)
            return Color.magenta;
        return c.Color;
    }
}