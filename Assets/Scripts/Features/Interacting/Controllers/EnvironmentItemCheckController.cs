using UnityEngine;

public class EnvironmentItemCheckController
{
    private SignalBus _signalBus;
    private PlayerView _player;
    private UpdateProvider _updateProvider;

    public EnvironmentItemCheckController(SignalBus signalBus, UpdateProvider updateProvider, PlayerView player)
    {
        _signalBus = signalBus;
        _player = player;
        _updateProvider = updateProvider;
        _updateProvider.Updates.Add(Check);
    }

    private void Check()
    {
        Vector3 origin = _player.transform.position+Vector3.up*4;

        if (Physics.SphereCast(origin, 1, -_player.transform.up, out RaycastHit hit, 10, LayerMask.GetMask("Interactable")))
        {
            if(hit.collider.TryGetComponent(out InteractableObject obj) && obj.IsActive)
            {
                _signalBus.FireSignal(new UpdateInteractableItemSignal(obj));
            }
            else
                _signalBus.FireSignal(new UpdateInteractableItemSignal(null));
        }
        else
            _signalBus.FireSignal(new UpdateInteractableItemSignal(null));
    }
}
