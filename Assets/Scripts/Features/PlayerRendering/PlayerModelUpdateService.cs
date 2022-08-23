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
    private PlayerModelForRendering _renderModel;
    private Dictionary<Item, GameObject[]> _cachedModels = new Dictionary<Item, GameObject[]>();
    private Dictionary<ItemSlot, GameObject[]> _equippedGear = new Dictionary<ItemSlot, GameObject[]>()
    {
        {ItemSlot.Weapon, null }
    };

    public PlayerModelUpdateService(SignalBus signalBus, PlayerModelForRendering renderModel, PlayerView player, EquipedWeaponOffsetConfig offsetConfig) : base(signalBus)
    {
        _renderModel = renderModel;
        _player = player;
        _offsetConfig = offsetConfig;
        signalBus.Subscribe<OnEquipedItemChangedSignal>(OnEquipementChanged, this);
    }

    private void OnEquipementChanged(OnEquipedItemChangedSignal obj)
    {
        if (_equippedGear.Keys.Contains(obj.Slot))
        {
            foreach(var ob in _equippedGear[obj.Slot])
                ob.SetActive(false);
        }

        if (_cachedModels.Keys.Contains(obj.Item))
        {
            foreach (var ob in _equippedGear[obj.Slot])
                ob.SetActive(true);
        }
        else
        {
            int level = _inventoryService.ItemsLevels[obj.Item.Id];
            var prefab = obj.Item.PrafabDef.Prefabs[level].Prefab;

            var newModel = GameObject.Instantiate(prefab, _player.WeaponsHolder);
            var newModel2 = GameObject.Instantiate(prefab, _renderModel.HandAnchor);
            switch (obj.Slot)
            {
                case ItemSlot.Weapon:
                    newModel.transform.localPosition = _offsetConfig.GetOffsetData(obj.Item.Id).PositionOffset;
                    newModel.transform.localEulerAngles = _offsetConfig.GetOffsetData(obj.Item.Id).RotationOffest;
                    newModel.transform.localScale = _offsetConfig.GetOffsetData(obj.Item.Id).Scale;
                    newModel2.transform.localPosition = _offsetConfig.GetOffsetData(obj.Item.Id).PositionOffset;
                    newModel2.transform.localEulerAngles = _offsetConfig.GetOffsetData(obj.Item.Id).RotationOffest;
                    newModel2.transform.localScale = _offsetConfig.GetOffsetData(obj.Item.Id).Scale;
                    break;
            }
            _cachedModels.Add(obj.Item, new GameObject[] { newModel.gameObject ,newModel2.gameObject});
        }
        _equippedGear[obj.Slot] = _cachedModels[obj.Item];
    }

    private void OnSceneLoadedEquip(Item item, ItemSlot slot)
    {
        int level = _inventoryService.ItemsLevels[item.Id];
        var prefab = item.PrafabDef.Prefabs[level-1].Prefab;

        var newModel = GameObject.Instantiate(prefab, _player.WeaponsHolder);
        var newModel2 = GameObject.Instantiate(prefab, _renderModel.HandAnchor);
        switch (slot)
        {
            case ItemSlot.Weapon:
                newModel.transform.localPosition = _offsetConfig.GetOffsetData(item.Id).PositionOffset;
                newModel.transform.localEulerAngles = _offsetConfig.GetOffsetData(item.Id).RotationOffest;
                newModel.transform.localScale = _offsetConfig.GetOffsetData(item.Id).Scale;
                newModel2.transform.localPosition = _offsetConfig.GetOffsetData(item.Id).PositionOffset;
                newModel2.transform.localEulerAngles = _offsetConfig.GetOffsetData(item.Id).RotationOffest;
                newModel2.transform.localScale = _offsetConfig.GetOffsetData(item.Id).Scale;
                break;
        }
        _cachedModels.Add(item, new GameObject[] { newModel.gameObject, newModel2.gameObject });
        _equippedGear[slot] = _cachedModels[item];
    }

    public override void OnServicesLoaded(params LoadableService[] services)
    {
        _inventoryService = services.First(s => s is InventoryService) as InventoryService;
        OnSceneLoadedEquip(_inventoryService.GetItem("WEAPON_SWORD_1"), ItemSlot.Weapon);
    }
}
