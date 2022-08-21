using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemConfig", menuName = "Configs/ItemConfig")]
public class Item : ScriptableObject
{
    public string ItemId;
    public ItemNameDef NameDef;
    public ItemGroupDef GroupDef;
    public ItemIconDef IconDef;
    public ItemRarityDef RarityDef;
    public ItemGearScoreDef GearScoreDef;
    public ItemLevelDef LevelDef;

    public List<Type> Definitions
    {
        get
        {
            List<Type> defs = new List<Type>();

            if (NameDef != null) defs.Add(NameDef.GetType());
            if (GroupDef != null) defs.Add(GroupDef.GetType());
            if (IconDef != null) defs.Add(IconDef.GetType());
            if (RarityDef != null) defs.Add(RarityDef.GetType());
            if (GearScoreDef.Mappings.Length>0) defs.Add(GearScoreDef.GetType());
            if (LevelDef.Level>0) defs.Add(LevelDef.GetType());

            return defs;
        }
    }
}
