
using System;
using UnityEngine;
using UnityEngine.UI;


public class GraphicColor : MonoBehaviour {

    public PalleteColor Color;

    public bool OverrideAlpha;
    
    [Range(0, 100)]
    public int Alpha;

    private Graphic _Graphic;

    private void Refresh() {
        RefreshReferences();
        RefreshValue();
    }

    private void RefreshReferences() {
        if (!_Graphic) {
            _Graphic = GetComponent<Graphic>();
        }
    }

    public void RefreshValue() {
        if (_Graphic == null)
            Debug.LogError(gameObject.name);
        _Graphic.color = OverrideAlpha ? new Color(Color.Color.r, Color.Color.g, Color.Color.b, Alpha/100f) : Color;
    }

    private void Awake() {
        Refresh();
    }

    private void OnValidate() {
        if (Color != null) {
            Color.MarkDirty();
            Refresh();
        }
    }
}

