using System;
using System.Linq;
using UnityEngine;
public class CollectButtonController : GameUiControllerBase
{
    private CollectButton _button;
    private Camera _camera;
    private InputService _inputService;
    private bool _isCollecting;

    public CollectButtonController(SignalBus signalBus, GameCanvas gameCanvas, Camera camera) : base(signalBus, gameCanvas)
    {
        _button = gameCanvas.GetView<GameUiPanel>().GetView<CollectButton>();
        _camera = camera;
        signalBus.Subscribe<UpdateCollectableItemSignal>(OnNearbyItemsUpdated,this);
        signalBus.Subscribe<OnServicesLoadedSignal>(OnServicesLoaded,this);
        signalBus.Subscribe<SendPlayerStatesSignal>(OnStateUpdated, this);
    }

    private void OnStateUpdated(SendPlayerStatesSignal signal)
    {
        _isCollecting = signal.States[PlayerState.Collecting];
    }

    private void OnServicesLoaded(OnServicesLoadedSignal obj)
    {
        _inputService = obj.Services.First(_ => _ is InputService) as InputService;
    }

    private void OnNearbyItemsUpdated(UpdateCollectableItemSignal signal)
    {
        if (signal.Object == null || _isCollecting)
        {
            _button.SetActive(false);
        }
        else
        {
            _button.SetData(_inputService.Collect.ToString());
            _button.transform.position = _camera.WorldToScreenPoint(signal.Object.transform.position);
            _button.SetActive(true);
        }
    }
}
