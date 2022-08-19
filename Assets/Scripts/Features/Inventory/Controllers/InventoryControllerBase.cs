public class InventoryControllerBase
{
    protected readonly SignalBus _signalBus;
    protected readonly ItemsMap _itemsMap;
    protected readonly InventoryService _inventoryService;

    public InventoryControllerBase(SignalBus signalBus, ItemsMap itemsMap, InventoryService inventoryService)
    {
        _signalBus = signalBus;
        _itemsMap = itemsMap;
        _inventoryService = inventoryService;
    }
}
