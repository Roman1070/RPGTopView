using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class PlayerArmedStateController : PlayerGearControllerBase
{
    private float _animationDuration = 0.8f;
    private Animator _animator;
    private PlayerStatesService _statesService;
    private Transform _handAnchor;
    private Transform _spineAnchor;
    private Transform _currentWeapon;
    private EquipedWeaponOffsetConfig _weaponOffsetConfig;
    private string _currentWeaponId;

    public PlayerArmedStateController(SignalBus signalBus, PlayerView player, PlayerStatesService statesService, EquipedWeaponOffsetConfig weaponOffsetConfig) : base(signalBus, player)
    {
        _statesService = statesService;
        _weaponOffsetConfig = weaponOffsetConfig;
        _handAnchor = player.HandAnchor;
        _spineAnchor = player.SpineAnchor;
        _currentWeapon = player.CurrentWeapon;
        _animator = _player.Model.GetComponent<Animator>();

        signalBus.Subscribe<OnInputDataRecievedSignal>(OnInput, this);
        signalBus.Subscribe<DrawWeaponSignal>(DrawWeapon, this);
        signalBus.Subscribe<UpdateEquipedItemsDataSignal>(UpdateEquipedItem, this);
    }

    private void UpdateEquipedItem(UpdateEquipedItemsDataSignal obj)
    {
        _currentWeaponId = obj.EquipedItems[ItemSlot.Weapon].Id;
    }

    private void DrawWeapon(DrawWeaponSignal obj)
    {
        if (obj.Draw)_player.StartCoroutine(DrawWeapon());
        else _player.StartCoroutine(RemoveWeapon());
    }

    private void OnInput(OnInputDataRecievedSignal obj)
    {
        if (obj.Data.ToggleArmedStatus) 
        {
            bool toggleAvailable = !(_statesService.States[PlayerState.Rolling]
                || _statesService.States[PlayerState.Interacting] || _statesService.States[PlayerState.Attacking]);

            if(toggleAvailable && !_statesService.States[PlayerState.DrawingWeapon])
                ToggleArmedStatus();
        }
    }

    private void ToggleArmedStatus()
    {
        if (_statesService.States[PlayerState.IsArmed]) _player.StartCoroutine(RemoveWeapon());
        else _player.StartCoroutine(DrawWeapon());
    }

    private IEnumerator DrawWeapon()
    {
        yield return new WaitUntil(() => !_statesService.States[PlayerState.DrawingWeapon]);

        _animator.SetTrigger("DrawWeapon");
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.DrawingWeapon, true));

        DOVirtual.DelayedCall(_animationDuration * 0.35f, () =>
        {
            _currentWeapon.SetParent(_handAnchor);
            _currentWeapon.transform.localPosition = _weaponOffsetConfig.GetOffsetData(_currentWeaponId).DrawnPosition;
            _currentWeapon.transform.localEulerAngles = _weaponOffsetConfig.GetOffsetData(_currentWeaponId).DrawnRotation;
        });

        DOVirtual.DelayedCall(_animationDuration + 0.3f, () =>
        {
            _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.IsArmed, true));
            _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.DrawingWeapon, false));
        }); 
    }

    private IEnumerator RemoveWeapon()
    {
        yield return new WaitUntil(() => !_statesService.States[PlayerState.DrawingWeapon]);

        _animator.SetTrigger("RemoveWeapon");
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.DrawingWeapon, true));
        DOVirtual.DelayedCall(_animationDuration, () =>
        {
            _currentWeapon.SetParent(_spineAnchor);
            _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.IsArmed, false));
            _currentWeapon.transform.DOLocalMove(_weaponOffsetConfig.GetOffsetData(_currentWeaponId).RemovedPosition, 0.2f);
            _currentWeapon.transform.localEulerAngles = _weaponOffsetConfig.GetOffsetData(_currentWeaponId).RemovedRotation;

            DOVirtual.DelayedCall(0.3f, () =>
            {
                _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.DrawingWeapon, false));
            });
        });
    }
}
