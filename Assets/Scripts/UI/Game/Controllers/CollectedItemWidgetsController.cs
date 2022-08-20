using System;
using System.Linq;

public class CollectedItemWidgetsController : GameUiControllerBase
{
    private ItemCollectedWidget[] _widgets;
    private InventoryService _inventoryService;

    public CollectedItemWidgetsController(SignalBus signalBus, GameCanvas gameCanvas) : base(signalBus, gameCanvas)
    {
        _widgets = gameCanvas.GetView<GameUiPanel>().GetViews<ItemCollectedWidget>().ToArray();
        signalBus.Subscribe<OnServicesLoadedSignal>(OnServicesLoaded,this);
        signalBus.Subscribe<OnItemCountChangedSignal>(ShowWidgets,this);
    }

    private void OnServicesLoaded(OnServicesLoadedSignal signal)
    {
        _inventoryService = signal.Services.First(t=>t is InventoryService) as InventoryService;
    }

    private void ShowWidgets(OnItemCountChangedSignal signal)
    {
        foreach (var widget in _widgets)
            widget.gameObject.SetActive(false);

        for (int i = 0; i < signal.Items.Length; i++)
        {
            _widgets[i].gameObject.SetActive(true);
            _widgets[i].SetItem(_inventoryService.GetItem(signal.Items[i].Id), signal.Items[i].Count);
            _widgets[i].PlayAnims();
        }
    }
}

