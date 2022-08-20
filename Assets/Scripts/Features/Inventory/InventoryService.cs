using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class InventoryService : LoadableService
{
    private ItemsMap _itemsMap;

    private Dictionary<string, int> _itemsCount;
    private Dictionary<string,int> ItemsCount
    {
        get
        {
            if (_itemsCount == null)
            {
                _itemsCount = new Dictionary<string, int>();
                for (int i = 0; i < _itemsMap.Items.Length; i++)
                {
                    _itemsCount.Add(_itemsMap.Items[i].Id, 0);
                }
            }
            return _itemsCount;
        }

        set
        {
            _itemsCount = ItemsCount;
        }
    }

    private List<InventoryControllerBase> _controllers;

    public InventoryService(SignalBus signalBus, ItemsMap itemsMap) : base(signalBus)
    {
        _itemsMap = itemsMap;
        _signalBus.Subscribe<OnItemCountChangedSignal>(ChangeItemCount, this);

        InitControllers();
    }

    public void InitControllers()
    {

    }

    private void ChangeItemCount(OnItemCountChangedSignal signal)
    {
        foreach(var item in signal.Items)
        {
            ItemsCount[item.Id] += item.Count;
        }

    }

    public int GetItemCount(string id) => ItemsCount[id];

    public Item GetItem(string id) => _itemsMap.Items.First(_ => _.Id == id).Item;
}
