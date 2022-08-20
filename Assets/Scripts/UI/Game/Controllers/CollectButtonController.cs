using System;
using System.Linq;
using UnityEngine;
public class CollectButtonController : GameUiControllerBase
{
    private CollectButton _button;
    private InputService _inputService;

    public CollectButtonController(SignalBus signalBus, GameCanvas gameCanvas) : base(signalBus, gameCanvas)
    {
        _button = gameCanvas.GetView<GameUiPanel>().GetView<CollectButton>();
        signalBus.Subscribe<UpdateCollectableItemSignal>(OnNearbyItemsUpdated,this);
        signalBus.Subscribe<OnServicesLoadedSignal>(OnServicesLoaded,this);
    }

    private void OnServicesLoaded(OnServicesLoadedSignal obj)
    {
        _inputService = obj.Services.First(_ => _ is InputService) as InputService;
    }

    private void OnNearbyItemsUpdated(UpdateCollectableItemSignal signal)
    {
        if (signal.Object == null)
        {
            _button.SetActive(false);
        }
        else
        {
            _button.SetData(_inputService.Collect.ToString());
            _button.transform.position = Camera.main.WorldToScreenPoint(signal.Object.transform.position);
            _button.SetActive(true);
        }
    }
}
