using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CameraOffsetConfig", menuName = "Configs/CameraOffsetConfig")]

public class CameraOffsetConfig : ScriptableObject
{
    public Vector3 PositionOffset;
    public Vector3 RotationOffset;
}
