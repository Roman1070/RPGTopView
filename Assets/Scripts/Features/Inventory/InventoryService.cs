using System.Collections.Generic;
using System.Linq;

public class InventoryService : LoadableService
{
    public ItemsMap ItemsMap { get; private set; }
    private Dictionary<string, int> _itemsCount;

    public Dictionary<string, int> ItemsCount {
        get
        {
            if (_itemsCount == null)
            {
                _itemsCount = new Dictionary<string, int>();
                foreach(var item in ItemsMap.Items)
                {
                    _itemsCount.Add(item.Id, 0);
                }
            }
            return _itemsCount;
        }
        private set { } }

    private List<InventoryControllerBase> _controllers;

    public InventoryService(SignalBus signalBus, ItemsMap itemsMap) : base(signalBus)
    {
        ItemsMap = itemsMap;
        _signalBus.Subscribe<OnItemCountChangedSignal>(ChangeItemCount, this);

        InitControllers();
    }

    public void InitControllers()
    {

    }

    private void ChangeItemCount(OnItemCountChangedSignal signal)
    {
        foreach (var item in signal.Items)
        {
            ItemsCount[item.Id] += item.Count;
        }

    }

    public int GetItemCount(string id) => ItemsCount[id];

    public Item GetItem(string id) => ItemsMap.Items.First(_ => _.Id == id).Item;
}
