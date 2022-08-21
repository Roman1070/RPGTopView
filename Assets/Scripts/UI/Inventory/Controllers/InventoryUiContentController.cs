using System;

public class InventoryUiContentController : InventoryUiControllerBase
{
    private ItemsMap ItemsMap => _inventoryService.ItemsMap;

    public InventoryUiContentController(SignalBus signalBus, GameCanvas gameCanvas, InventoryService inventoryService) : base(signalBus, gameCanvas, inventoryService)
    {
        UpdatePanel();
        signalBus.Subscribe<SetActivePanelSignal>(UpdatePanel,this);
    }

    private void UpdatePanel(SetActivePanelSignal obj=null)
    {
        if (obj != null && obj.PanelType != typeof(InventoryPanel)) return;

        var rewardTabViews = _gameCanvas.GetView<InventoryPanel>().GetView<TabsView>().TabMappings[0].Content.GetViews<ItemWidgetView>();
        foreach (var item in rewardTabViews) item.SetActive(false);

        for (int i = 0; i < ItemsMap.Items.Length; i++)
        {
            var item = ItemsMap.Items[i];
            var view = rewardTabViews[i];
            int count = _inventoryService.GetItemCount(item.Id);
            if (count > 0)
            {
                view.SetActive(true);
                if (item.Item.Definitions.Contains(typeof(ItemIconDef))) view.SetIcon(item.Item.IconDef.Icon);
                if (item.Item.Definitions.Contains(typeof(ItemRarityDef))) view.SetRarity(item.Item.RarityDef.Rarity);
                if (item.Item.Definitions.Contains(typeof(ItemGearScoreDef)))
                {
                    int gs = item.Item.GearScoreDef.Mappings[item.Item.LevelDef.Level].GearScore;
                    view.SetGearScore(gs);
                }
                view.SetCount(count);
            }
        }
    }
}
