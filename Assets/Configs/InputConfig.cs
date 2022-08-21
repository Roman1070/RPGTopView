using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InputConfig", menuName = "Configs/InputConfig")]
public class InputConfig : ScriptableObject
{
    public float MouseSensitivityX;
    public float MouseSensitivityY;

    public KeyCode Right = KeyCode.D;
    public KeyCode Left = KeyCode.A;
    public KeyCode Up = KeyCode.W;
    public KeyCode Down = KeyCode.S;
    public KeyCode Jump = KeyCode.Space;
    public KeyCode Sprint = KeyCode.LeftShift;
    public KeyCode Attack = KeyCode.Mouse0;
    public KeyCode Roll = KeyCode.LeftControl;
    public KeyCode Collect = KeyCode.E;
    public KeyCode DevConsole = KeyCode.KeypadMinus;
    public KeyCode Inventory = KeyCode.I;
    public KeyCode ToggleArmedStatus = KeyCode.Alpha1;
}
