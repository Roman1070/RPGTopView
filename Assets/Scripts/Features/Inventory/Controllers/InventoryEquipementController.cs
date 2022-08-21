using DG.Tweening;
using System;
using System.Collections.Generic;

public class InventoryEquipementController : InventoryControllerBase
{
    private Dictionary<ItemSlot, Item> _equippedItems;
    public InventoryEquipementController(SignalBus signalBus, ItemsMap itemsMap, InventoryService inventoryService) : base(signalBus, itemsMap, inventoryService)
    {
        _equippedItems = new Dictionary<ItemSlot, Item>()
        {
            { ItemSlot.Weapon, null},
            { ItemSlot.Helmet, null},
            { ItemSlot.Body, null},
            { ItemSlot.Gloves, null},
            { ItemSlot.Legs, null},
            { ItemSlot.Boots, null}
        };
        signalBus.Subscribe<OnEquipedItemChangedSignal>(OnEquipItem, this);
        DOVirtual.DelayedCall(0.1f,()=> _signalBus.FireSignal(new UpdateEquipedItemsDataSignal(_equippedItems)));
    }

    private void OnEquipItem(OnEquipedItemChangedSignal obj)
    {
        _equippedItems[obj.Slot] = obj.Item;
        _signalBus.FireSignal(new UpdateEquipedItemsDataSignal(_equippedItems));
    }
}
