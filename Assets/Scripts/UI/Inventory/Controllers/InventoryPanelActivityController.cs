using System;
using UnityEngine;

public class InventoryPanelActivityController : InventoryUiControllerBase
{
    public InventoryPanelActivityController(SignalBus signalBus, GameCanvas gameCanvas, InventoryService inventoryService) : base(signalBus, gameCanvas, inventoryService)
    {
        signalBus.Subscribe<OnInputDataRecievedSignal>(OnInput, this);
    }

    private void OnInput(OnInputDataRecievedSignal obj)
    {
        if (obj.Data.InventoryCall)
        {
            _signalBus.FireSignal(new SetActivePanelSignal(typeof(InventoryPanel)));
        }
        if (obj.Data.Esc)
        {
            _signalBus.FireSignal(new BackToPreviuosPanelSignal());
        }
    }
}