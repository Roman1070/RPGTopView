using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "EquipedWeaponOffsetConfig", menuName = "Configs/EquipedWeaponOffsetConfig")]
public class EquipedWeaponOffsetConfig : ScriptableObject
{
    public WeaponOffsetData[] Offsets;

    public WeaponOffsetData GetOffsetData(string id) => Offsets.First(d => d.Id == id);
}

[Serializable]
public class WeaponOffsetData
{
    public string Id;
    public Vector3 DrawnPosition;
    public Vector3 DrawnRotation;
    public Vector3 RemovedPosition;
    public Vector3 RemovedRotation;
    public Vector3 Scale;
}
