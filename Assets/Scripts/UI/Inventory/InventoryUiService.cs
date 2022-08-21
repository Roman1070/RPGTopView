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
        _signalBus.Subscribe<OnServicesLoadedSignal>(OnServicesLoaded, this);
    }

    private void InitControllers()
    {
        _controllers = new List<InventoryUiControllerBase>()
        {
            new InventoryPanelActivityController(_signalBus,_gameCanvas,_inventoryService),
        };
    }

    private void OnServicesLoaded(OnServicesLoadedSignal obj)
    {
        _inventoryService = obj.Services.First(_ => _ is InventoryService) as InventoryService;
        InitControllers();
    }
}
