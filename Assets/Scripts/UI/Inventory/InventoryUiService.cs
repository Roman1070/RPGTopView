using System.Collections.Generic;
using System.Linq;

public class InventoryUiService : LoadableService
{
    private GameCanvas _gameCanvas;
    private InventoryService _inventoryService;
    private List<InventoryUiControllerBase> _controllers;

    public InventoryUiService(SignalBus signalBus, GameCanvas gameCanvas) : base(signalBus)
    {
        _gameCanvas = gameCanvas;
    }

    public override void OnServicesLoaded(params LoadableService[] services)
    {
        _inventoryService = services.First(s => s.GetType() == typeof(InventoryService)) as InventoryService;
        InitControllers();
    }

    private void InitControllers()
    {
        _controllers = new List<InventoryUiControllerBase>()
        {
            new InventoryPanelActivityController(_signalBus,_gameCanvas,_inventoryService),
            new InventoryUiContentController(_signalBus,_gameCanvas,_inventoryService)
        };
    }
}
