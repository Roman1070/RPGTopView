using UnityEngine;

public enum ItemSlot
{
    Weapon,
    Helmet,
    Body,
    Gloves,
    Legs,
    Boots,
    Consumable1,
    Consumable2
}

public class OnEquipedItemChangedSignal : ISignal
{
    public Item Item;
    public ItemSlot Slot;

    public OnEquipedItemChangedSignal(Item item, ItemSlot slot)
    {
        Item = item;
        Slot = slot;
    }
}
