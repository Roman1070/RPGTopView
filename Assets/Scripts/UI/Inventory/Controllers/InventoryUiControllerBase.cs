using System;

public class InventoryUiControllerBase
{
    protected SignalBus _signalBus;
    protected GameCanvas _gameCanvas;
    protected InventoryService _inventoryService;

    public InventoryUiControllerBase(SignalBus signalBus, GameCanvas gameCanvas, InventoryService inventoryService)
    {
        _signalBus = signalBus;
        _gameCanvas = gameCanvas;
        _inventoryService = inventoryService;
    }
}
