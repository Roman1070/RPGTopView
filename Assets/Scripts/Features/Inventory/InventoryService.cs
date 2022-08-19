using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryService : LoadableService
{
    [SerializeField]
    private ItemsMap _itemsMap;

    private ItemsCount[] _itemsCount;

    public override void Init()
    {
        _signalBus.Subscribe<OnItemCountChangedSignal>(ChangeItemCount,this);
    }

    private void ChangeItemCount(OnItemCountChangedSignal signal)
    {
        _itemsCount.First(_ => _.Id == signal.Id).SetCount(signal.Value);
    }

    public ItemConfig GetItem(string id)
    {
        return _itemsMap.Items.First(_ => _.Id == id).Item;
    }
}
