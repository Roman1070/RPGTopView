using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraMovementConfig", menuName = "Configs/CameraMovementConfig")]
public class CameraMovementConfig : ScriptableObject
{
    public Vector2 Sensitivity;
    public int MinAngle;
    public int MaxAngle;
    public bool InvertX;
    public bool InvertY;
    public Vector3 CameraOffset;
}
