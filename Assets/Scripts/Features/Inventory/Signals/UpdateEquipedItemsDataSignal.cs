using System.Collections.Generic;

public class UpdateEquipedItemsDataSignal : ISignal
{
    public Dictionary<ItemSlot, Item> EquipedItems;

    public UpdateEquipedItemsDataSignal(Dictionary<ItemSlot, Item> equipedItems)
    {
        EquipedItems = equipedItems;
    }
}
