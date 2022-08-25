using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class PlayerModelUpdateService : LoadableService
{
    private InventoryService _inventoryService;
    private PlayerView _player;
    private EquipedWeaponOffsetConfig _offsetConfig;
    private RenderSpace _renderModel;
    private Dictionary<Item, GameObject[]> _cachedModels = new Dictionary<Item, GameObject[]>();
    private Dictionary<ItemSlot, GameObject[]> _equippedGear = new Dictionary<ItemSlot, GameObject[]>()
    {
        {ItemSlot.Weapon, null }
    };

    public PlayerModelUpdateService(SignalBus signalBus, RenderSpace renderModel, PlayerView player, EquipedWeaponOffsetConfig offsetConfig) : base(signalBus)
    {
        _renderModel = renderModel;
        _player = player;
        _offsetConfig = offsetConfig;
        signalBus.Subscribe<OnEquipedItemChangedSignal>(OnEquipementChanged, this);
    }

    private void OnEquipementChanged(OnEquipedItemChangedSignal obj)
    {
        if (_equippedGear.Keys.Contains(obj.Slot) && _equippedGear[obj.Slot]!=null)
        {
            foreach(var ob in _equippedGear[obj.Slot])
                ob.SetActive(false);
        }

        if (_cachedModels.Keys.Contains(obj.Item))
        {
            foreach (var ob in _cachedModels[obj.Item])
                 ob.SetActive(true);
        }
        else
        {
            int level = _inventoryService.ItemsLevels[obj.Item.Id];
            var prefab = obj.Item.PrafabDef.Prefabs[level-1].Prefab;

            var newModels = new GameObject[] { GameObject.Instantiate(prefab, _player.WeaponsHolder), GameObject.Instantiate(prefab, _renderModel.HandAnchor) };
            switch (obj.Slot)
            {
                case ItemSlot.Weapon:
                    foreach(var model in newModels)
                    {
                        model.transform.localPosition = _offsetConfig.GetOffsetData(obj.Item.Id).PositionOffset;
                        model.transform.localEulerAngles = _offsetConfig.GetOffsetData(obj.Item.Id).RotationOffest;
                        model.transform.localScale = _offsetConfig.GetOffsetData(obj.Item.Id).Scale;
                    }
                    break;
            }
            _cachedModels.Add(obj.Item, newModels);
        }
        _equippedGear[obj.Slot] = _cachedModels[obj.Item];
    }

    public override void OnServicesLoaded(params LoadableService[] services)
    {
        _inventoryService = services.First(s => s is InventoryService) as InventoryService;
        _signalBus.FireSignal(new OnEquipedItemChangedSignal(_inventoryService.GetItem("WEAPON_SWORD_1"), ItemSlot.Weapon));
    }
}
