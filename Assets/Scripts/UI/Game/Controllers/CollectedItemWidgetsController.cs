using System;
using System.Linq;

public class CollectedItemWidgetsController : GameUiControllerBase
{
    private ItemCollectedWidget[] _widgets;
    private InventoryService _inventoryService;

    public CollectedItemWidgetsController(SignalBus signalBus, GameCanvas gameCanvas, InventoryService inventory) : base(signalBus, gameCanvas)
    {
        _inventoryService = inventory;
        _widgets = gameCanvas.GetView<GameUiPanel>().GetViews<ItemCollectedWidget>().ToArray();
        _signalBus.Subscribe<OnItemCountChangedSignal>(ShowWidgets, this);
    }

    private void ShowWidgets(OnItemCountChangedSignal signal)
    {
        foreach (var widget in _widgets)
            widget.gameObject.SetActive(false);

        for (int i = 0; i < signal.Items.Length; i++)
        {
            _widgets[i].gameObject.SetActive(true);
            if (_inventoryService.GetItem(signal.Items[i].Id).GroupDef.Group == ItemGroup.Resource)
            {
                _widgets[i].SetItem(_inventoryService.GetItem(signal.Items[i].Id), signal.Items[i].Count);
            }
            if (_inventoryService.GetItem(signal.Items[i].Id).GroupDef.Group == ItemGroup.Weapon)
            {
                _widgets[i].SetItem(_inventoryService.GetItem(signal.Items[i].Id), signal.Items[i].Count,_inventoryService);
            }
            _widgets[i].PlayAnims();
        }
    }
}

