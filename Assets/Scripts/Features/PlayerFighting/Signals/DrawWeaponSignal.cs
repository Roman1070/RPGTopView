using UnityEngine;

public class DrawWeaponSignal : ISignal
{
    public bool Draw;

    public DrawWeaponSignal(bool draw)
    {
        Draw = draw;
    }
}
