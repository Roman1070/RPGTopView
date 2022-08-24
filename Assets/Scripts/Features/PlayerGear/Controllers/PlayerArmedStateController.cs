using DG.Tweening;
using System;
using System.Collections;
using UnityEngine;

public class PlayerArmedStateController : PlayerGearControllerBase
{
    private float _animationDuration = 0.6f;
    private Animator _animator;
    private PlayerStatesService _statesService;
    private Transform _handAnchor;
    private Transform _spineAnchor;
    private Transform _weaponHolder;

    private Vector3 DrawnPosition => new Vector3(-0.007f, 0.067f , 0);
    private Vector3 RemovedPosition => new Vector3(-0.188f, 0.325f , -0.06f);
    private Vector3 RemovedRotation => new Vector3(103, 94 , 18);

    public PlayerArmedStateController(SignalBus signalBus, PlayerView player, PlayerStatesService statesService, EquipedWeaponOffsetConfig weaponOffsetConfig) : base(signalBus, player)
    {
        _statesService = statesService;
        _handAnchor = player.HandAnchor;
        _spineAnchor = player.SpineAnchor;
        _weaponHolder = player.WeaponsHolder;
        _animator = _player.Model.GetComponent<Animator>();

        signalBus.Subscribe<OnInputDataRecievedSignal>(OnInput, this);
        signalBus.Subscribe<DrawWeaponSignal>(DrawWeapon, this);

        _weaponHolder.SetParent(_spineAnchor);
        _weaponHolder.transform.localPosition = RemovedPosition;
        _weaponHolder.transform.localEulerAngles = RemovedRotation;
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
        yield return new WaitUntil(() => !_statesService.States[PlayerState.DrawingWeapon] && !_statesService.States[PlayerState.Attacking]);

        _animator.SetTrigger("DrawWeapon");
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.DrawingWeapon, true));

        DOVirtual.DelayedCall(_animationDuration * 0.35f, () =>
        {
            _weaponHolder.SetParent(_handAnchor);
            _weaponHolder.transform.localPosition = DrawnPosition;
            _weaponHolder.transform.localEulerAngles = Vector3.zero;
        });

        DOVirtual.DelayedCall(_animationDuration + 0.3f, () =>
        {
            _animator.SetBool("IsArmed", true);
            _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.IsArmed, true));
            _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.DrawingWeapon, false));
        }); 
    }

    private IEnumerator RemoveWeapon()
    {
        yield return new WaitUntil(() => !_statesService.States[PlayerState.DrawingWeapon] && !_statesService.States[PlayerState.Attacking]);

        _animator.SetTrigger("RemoveWeapon");
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.DrawingWeapon, true));
        DOVirtual.DelayedCall(_animationDuration+0.2f, () =>
        {
            _weaponHolder.SetParent(_spineAnchor);
            _weaponHolder.transform.DOLocalMove(RemovedPosition, 0.2f);
            _weaponHolder.transform.localEulerAngles = RemovedRotation;

            DOVirtual.DelayedCall(0.3f, () =>
            {
                _animator.SetBool("IsArmed", false);
                _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.DrawingWeapon, false));
                _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.IsArmed, false));
            });
        });
    }
}
