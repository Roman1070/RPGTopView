using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemConfig", menuName = "Configs/ItemConfig")]
public class ItemConfig : ScriptableObject
{
    public string ItemId;
    public ItemNameDef Name;
    public ItemGroupDef Group;
}
