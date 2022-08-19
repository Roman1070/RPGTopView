using System;
using UnityEngine;

[CreateAssetMenu(fileName = "ItemsMap", menuName = "Configs/ItemsMap")]
public class ItemsMap : ScriptableObject
{
    public IdentifiedItem[] Items;
}

[Serializable]
public struct IdentifiedItem
{
    public string Id;
    public Item Item;
}