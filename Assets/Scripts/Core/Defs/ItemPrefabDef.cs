using System;
using UnityEngine;

[Serializable]
public class ItemPrefabDef : Def
{
    public ItemLevelPrefabMapping[] Prefabs;
}

[Serializable]
public class ItemLevelPrefabMapping
{
    public int Level;
    public GameObject Prefab;
    public Sprite Icon;
}
