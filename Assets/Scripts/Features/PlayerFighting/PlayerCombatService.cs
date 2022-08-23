using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum WeaponType
{
    Disarmed,
    TwoHanded,
}

public class PlayerCombatService : LoadableService
{
    public WeaponType CurrentWeaponType { get; private set; }

    private PlayerView _player;
    private PlayerCombatConfig _config;
    private List<PlayerCombatControllerBase> _controllers;
    private PlayerStatesService _playerStatesService;
    private UpdateProvider _updateProvider;
    protected Inventory _inventory;

    public PlayerCombatService(SignalBus signalBus, PlayerView player, PlayerCombatConfig config, UpdateProvider updateProvider)
        : base(signalBus)
    {
        _player = player;
        _config = config;
        _updateProvider = updateProvider;
        CurrentWeaponType = WeaponType.Disarmed;
    }


    public override void OnServicesLoaded(params LoadableService[] services)
    {
        _playerStatesService = services.First(s => s.GetType() == typeof(PlayerStatesService)) as PlayerStatesService;
        _inventory = (services.First(s => s.GetType() == typeof(InventoryService)) as InventoryService).Inventory;
        _signalBus.Subscribe<OnPlayerStateChangedSignal>(OnWeaponDrawn, this);
        InitControllers();
    }

    private void OnWeaponDrawn(OnPlayerStateChangedSignal obj)
    {
        if (obj.State==PlayerState.IsArmed)
        {
            CurrentWeaponType = obj.Value ? _inventory.CurrentWeaponType : WeaponType.Disarmed;
        }
    }

    private void InitControllers()
    {
        _controllers = new List<PlayerCombatControllerBase>()
        {
            new PlayerThoHandedWeaponCombatController(_signalBus,_player,_config,_playerStatesService,_updateProvider,this),
            new PlayerDisarmedCombatController(_signalBus,_player,_config,_playerStatesService,_updateProvider,this),
        };
    }
}
