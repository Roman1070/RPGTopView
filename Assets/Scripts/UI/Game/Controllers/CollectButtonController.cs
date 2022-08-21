using System;
using System.Linq;
using UnityEngine;
public class CollectButtonController : GameUiControllerBase
{
    private CollectButton _button;
    private Camera _camera;
    private InputService _inputService;
    private PlayerStatesService _states;

    public CollectButtonController(SignalBus signalBus, GameCanvas gameCanvas, Camera camera, PlayerStatesService states, InputService inputService) : base(signalBus, gameCanvas)
    {
        _button = gameCanvas.GetView<GameUiPanel>().GetView<CollectButton>();
        _camera = camera;
        _states = states;
        _inputService = inputService;
        _signalBus.Subscribe<UpdateCollectableItemSignal>(OnNearbyItemsUpdated, this);
    }

    private void OnNearbyItemsUpdated(UpdateCollectableItemSignal signal)
    {
        if (signal.Object == null || _states.States[PlayerState.Collecting])
        {
            _button.SetActive(false);
        }
        else
        {
            _button.SetData(_inputService.Config.Collect.ToString(), "Collect");
            _button.transform.position = _camera.WorldToScreenPoint(signal.Object.transform.position);
            _button.SetActive(true);
        }
    }
}
