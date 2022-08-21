using System;
using UnityEngine;

public class SetActivePanelSignal : ISignal
{
    public Type PanelType;

    public SetActivePanelSignal(Type panel)
    {
        PanelType = panel;
    }
}
