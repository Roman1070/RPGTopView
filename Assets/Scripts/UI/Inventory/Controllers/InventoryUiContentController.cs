using System.Collections.Generic;
using System.Linq;

public class InventoryUiContentController : InventoryUiControllerBase
{
    private ItemsMap ItemsMap => _inventoryService.ItemsMap;

    private Dictionary<string, bool> _itemEquiped;

    public InventoryUiContentController(SignalBus signalBus, GameCanvas gameCanvas, InventoryService inventoryService) : base(signalBus, gameCanvas, inventoryService)
    {
        _itemEquiped = new Dictionary<string, bool>();
        foreach (var item in ItemsMap.Items)
            _itemEquiped.Add(item.Id, false);

        signalBus.Subscribe<SetActivePanelSignal>(UpdatePanel, this);
        signalBus.Subscribe<UpdateEquipedItemsDataSignal>(OnEquipementChanged, this);

        UpdatePanel();
    }

    private void OnEquipementChanged(UpdateEquipedItemsDataSignal obj)
    {
        foreach (var item in ItemsMap.Items)
        {
            _itemEquiped[item.Id] = obj.EquipedItems.Values.ToList().Contains(item.Item);
        }

        var views = _gameCanvas.GetView<InventoryPanel>().GetView<EquipementBlockView>().GetViews<ItemWidgetView>();
        for(int i = 0; i < obj.EquipedItems.Keys.Count; i++)
        {
            var item = obj.EquipedItems[(ItemSlot)i];
            views[i].SetActive(item != null);

            if (item != null)
            {
                views[i].SetGearScore(_inventoryService.GetGearScore(item.Id));
                views[i].SetIcon(_inventoryService.GetIcon(item.Id));
                views[i].SetRarity(item.RarityDef.Rarity);
                views[i].SetCount(1);
            }
        }

        UpdatePanel();
    }

    private void UpdatePanel(SetActivePanelSignal signal = null)
    {
        if (signal != null && signal.PanelType != typeof(InventoryPanel)) return;

        UpdateTab(0, ItemGroup.MeleeWeapon);
        UpdateTab(1, ItemGroup.Resource);
    }

    private void UpdateTab(int index, ItemGroup group)
    {
        var itemWidgetViews = _gameCanvas.GetView<InventoryPanel>().GetView<TabsView>().TabMappings[index].Content.GetViews<ItemWidgetView>();
        foreach (var item in itemWidgetViews) item.SetActive(false);

        var items = ItemsMap.Items.Where(i => i.Item.GroupDef.Group == group && !_itemEquiped[i.Id]).ToArray();

        for (int i = 0; i < items.Length; i++)
        {
            var item = items[i];
            var view = itemWidgetViews[i];
            int count = _inventoryService.GetItemCount(item.Id);
            if (count > 0 && !_itemEquiped[item.Id])
            {
                view.SetActive(true);
                view.SetCount(count);
                view.SetIcon(_inventoryService.GetIcon(item.Item.Id));
                view.SetRarity(item.Item.RarityDef.Rarity);

                if (index == 0) SetActvieWeaponSlot(view, item);
            }
        }
    }


    private void SetActvieWeaponSlot(ItemWidgetView view, IdentifiedItem item)
    {
        if (item.Item.Definitions.Contains(typeof(ItemGearScoreDef)))
        {
            int gs = _inventoryService.GetGearScore(item.Id);
            view.SetGearScore(gs);
        }

        var button = view.GetView<EquipButtonView>().Button;
        button.onClick.RemoveAllListeners();
        button.onClick.AddListener(() =>
        {
            _signalBus.FireSignal(new OnEquipedItemChangedSignal(item.Item, ItemSlot.Weapon));
        });
    }
}
