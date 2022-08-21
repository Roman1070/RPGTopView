using System;
using System.Linq;
using UnityEngine;
public class InteractButtonController : GameUiControllerBase
{
    private InteractButton _button;
    private Camera _camera;
    private InputService _inputService;
    private PlayerStatesService _states;

    public InteractButtonController(SignalBus signalBus, GameCanvas gameCanvas, Camera camera, PlayerStatesService states, InputService inputService) : base(signalBus, gameCanvas)
    {
        _button = gameCanvas.GetView<GameUiPanel>().GetView<InteractButton>();
        _camera = camera;
        _states = states;
        _inputService = inputService;
        _signalBus.Subscribe<UpdateInteractableItemSignal>(OnNearbyItemsUpdated, this);
    }

    private void OnNearbyItemsUpdated(UpdateInteractableItemSignal signal)
    {
        if (signal.Object == null || _states.States[PlayerState.Interacting])
        {
            _button.SetActive(false);
        }
        else
        {
            _button.SetData(_inputService.Config.Collect.ToString(), signal.Object.Action);
            _button.transform.position = _camera.WorldToScreenPoint(signal.Object.transform.position);
            _button.SetActive(true);
        }
    }
}
