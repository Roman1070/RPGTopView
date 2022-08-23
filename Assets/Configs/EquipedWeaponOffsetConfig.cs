using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipedWeaponOffsetConfig", menuName = "Configs/EquipedWeaponOffsetConfig")]
public class EquipedWeaponOffsetConfig : ScriptableObject
{
    [SerializeField]
    private WeaponOffsetData[] Offsets;

    public WeaponOffsetData GetOffsetData(string id) => Offsets.First(d => d.Id == id);
}

[Serializable]
public class WeaponOffsetData
{
    public string Id;
    public Vector3 PositionOffset;
    public Vector3 RotationOffest;
    public Vector3 Scale;
}
