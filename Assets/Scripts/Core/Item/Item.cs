using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemConfig", menuName = "Configs/ItemConfig")]
public class Item : ScriptableObject
{
    public string ItemId;
    public ItemNameDef NameDef;
    public ItemGroupDef GroupDef;
}
