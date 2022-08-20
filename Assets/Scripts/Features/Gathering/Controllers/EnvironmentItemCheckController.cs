﻿using UnityEngine;

public class EnvironmentItemCheckController : ItemCollectControllerBase
{
    private PlayerView _player;
    private UpdateProvider _updateProvider;

    public EnvironmentItemCheckController(SignalBus signalBus, UpdateProvider updateProvider, PlayerView player) : base(signalBus)
    {
        _player = player;
        _updateProvider = updateProvider;
        _updateProvider.Updates.Add(Check);
    }

    private void Check()
    {
        Vector3 origin = _player.transform.position+Vector3.up*2;

        if (Physics.SphereCast(origin, 1, - _player.transform.up, out RaycastHit hit, 3))
        {
            _signalBus.FireSignal(new UpdateCollectableItemSignal(hit.collider.TryGetComponent(out CollectableObject obj) ? obj : null));
        }
    }
}