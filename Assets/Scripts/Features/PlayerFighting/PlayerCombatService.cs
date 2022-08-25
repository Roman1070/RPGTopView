using System;
using System.Collections.Generic;
using System.Linq;

public enum WeaponType
{
    OneHanded,
    TwoHanded,
}

public class PlayerCombatService : LoadableService
{
    public AttackType CurrentAttackType { get; private set; }

    private PlayerView _player;
    private PlayerCombatConfig _config;
    private List<PlayerCombatControllerBase> _controllers;
    private PlayerStatesService _playerStatesService;
    private UpdateProvider _updateProvider;
    protected Inventory _inventory;
    protected MainCameraAnchor _cameraAnchor;

    public PlayerCombatService(SignalBus signalBus, PlayerView player, PlayerCombatConfig config, UpdateProvider updateProvider, MainCameraAnchor cameraAnchor)
        : base(signalBus)
    {
        _player = player;
        _config = config;
        _updateProvider = updateProvider;
        _cameraAnchor = cameraAnchor;
        CurrentAttackType = AttackType.OneHanded;
    }


    public override void OnServicesLoaded(params LoadableService[] services)
    {
        _playerStatesService = services.First(s => s.GetType() == typeof(PlayerStatesService)) as PlayerStatesService;
        _inventory = (services.First(s => s.GetType() == typeof(InventoryService)) as InventoryService).Inventory;
        _signalBus.Subscribe<OnPlayerStateChangedSignal>(OnWeaponDrawn, this);
        _signalBus.Subscribe<OnEquipedItemChangedSignal>(OnEquipementChanged, this);
        CurrentAttackType = _playerStatesService.States[PlayerState.IsArmed]? (AttackType)_inventory.CurrentWeaponType : AttackType.Disarmed;
        InitControllers();
    }

    private void OnEquipementChanged(OnEquipedItemChangedSignal obj)
    {
        CurrentAttackType = _playerStatesService.States[PlayerState.IsArmed] ? (AttackType)_inventory.CurrentWeaponType : AttackType.Disarmed;
    }

    private void OnWeaponDrawn(OnPlayerStateChangedSignal obj)
    {
        CurrentAttackType = _playerStatesService.States[PlayerState.IsArmed] ? (AttackType)_inventory.CurrentWeaponType : AttackType.Disarmed;
    }

    private void InitControllers()
    {
        _controllers = new List<PlayerCombatControllerBase>()
        {
            new PlayerDisarmedCombatController(_signalBus,_player,_config,_playerStatesService,_updateProvider,this,_inventory,_cameraAnchor),
            new PlayerOneHandedWeaponCombatController(_signalBus,_player,_config,_playerStatesService,_updateProvider,this,_inventory,_cameraAnchor),
            new PlayerTwoHandedWeaponCombatController(_signalBus,_player,_config,_playerStatesService,_updateProvider,this,_inventory,_cameraAnchor)
        };
    }
}
