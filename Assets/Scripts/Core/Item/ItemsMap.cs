using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsMap",menuName = "Configs/ItemsMap")]
public class ItemsMap : ScriptableObject
{
    public IdentifiedItem[] Items;
}

[Serializable]
public struct IdentifiedItem
{
    public string Id;
    public ItemConfig Item;
}

[Serializable]
public struct ItemsCount
{
    public string Id;
    public int Count;

    public void SetCount(int value)
    {
        Count = value;
    }
}
