using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class MonoContainer<TMono> : View where TMono : MonoBehaviour {
    public List<TMono> Views;

    private readonly Dictionary<Type, TMono> _ViewCache = new Dictionary<Type, TMono>();
    protected virtual void OnValidate() {
        Views = GetComponentsInChildren<TMono>(true).Where(_ => _ != this).ToList();
        OnPostValidate();
    }
    protected virtual void OnPostValidate() { }
    public void Validate() {
        OnValidate();
    }
    
    public TView GetView<TView>() where TView : class {
        var type = typeof(TView);
        if (!_ViewCache.TryGetValue(type, out var view)) {
            var v = Views.OfType<TView>().FirstOrDefault();
            view = v as TMono;
            _ViewCache[type] = view;
        }

        return view as TView;
    }

    public bool GetView<TView>(out TView view) where TView : class
    {
        view = GetView<TView>();
        return view != null;
    }

    public List<TView> GetViews<TView>() {
        return Views.OfType<TView>().ToList();
    }

    public void AddView<TView>(TView view) where TView : TMono {
        Views.Add(view);
        _ViewCache[typeof(TView)] = view;
    }
}
