using System;

public class InventoryUiControllerBase
{
    protected SignalBus _signalBus;
    private GameCanvas _gameCanvas;
    private InventoryService _inventoryService;

    public InventoryUiControllerBase(SignalBus signalBus, GameCanvas gameCanvas, InventoryService inventoryService)
    {
        _signalBus = signalBus;
        _gameCanvas = gameCanvas;
        _inventoryService = inventoryService;
    }
}
