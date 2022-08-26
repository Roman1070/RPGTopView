using DG.Tweening;
using System.Collections;
using UnityEngine;

public enum AttackType
{
    OneHanded,
    TwoHanded,
    Disarmed, //Disarmed всегда в конец, здесь порядок такой же как и в WeaponType
}

public class PlayerCombatControllerBase
{
    protected SignalBus _signalBus;
    protected MainCameraAnchor _cameraAnchor;
    protected PlayerView _player;
    protected PlayerCombatConfig _config;
    protected PlayerStatesService _states;
    protected Animator _animator;
    protected PlayerAttackData _currentAttack;
    protected PlayerAttackData _previousAttack;
    protected PlayerAttackData _nextAttack;
    protected UpdateProvider _updateProvider;
    protected float _currentAttackProgress;
    protected bool _isDuringTransaction;
    protected Tween _onEndFight;
    protected PlayerCombatService _combatService;
    protected Inventory _inventory;
    protected Vector2Int _lastDirection;

    protected virtual string CurrentLayerName { get; }
    protected virtual AttackType TargetAttackType { get; }

    private readonly string[] LayersToToggle = new string[]
    {
        "MovementLayer",
        "InteractingLayer",
        "DrawWeapon"
    };
    private readonly string[] CombatLayers = new string[]
    {
        "CombatLayerTwoHanded",
        "CombatLayerDisarmed",
        "CombatLayerOneHanded",
    };

    protected float CurrentAttackNormalizedProgress
    {
        get
        {
            if (_currentAttack == null) return 0;

            return _currentAttackProgress / _currentAttack.Duration;
        }
    }

    private bool AttackAvailable => new bool[]
    {
        !_states.States[PlayerState.Grounded],
        _states.States[PlayerState.Rolling],
        _states.States[PlayerState.DrawingWeapon],
        _states.States[PlayerState.Interacting],
        _states.States[PlayerState.BrowsingUI],
    }.Sum().Inverse() && !_isDuringTransaction;

    public PlayerCombatControllerBase(SignalBus signalBus, PlayerView player, PlayerCombatConfig config, PlayerStatesService states,
        UpdateProvider updateProvider, PlayerCombatService combatService, Inventory inventory, MainCameraAnchor cameraAnchor)
    {
        _signalBus = signalBus;
        _player = player;
        _config = config;
        _combatService = combatService;
        _inventory = inventory;
        _config = config;
        _states = states;
        _updateProvider = updateProvider;
        _cameraAnchor = cameraAnchor;
        _animator = player.Model.GetComponent<Animator>();

        _updateProvider.Updates.Add(Update);
        _animator.SetLayerWeight(_animator.GetLayerIndex(CurrentLayerName), 0);
        signalBus.Subscribe<OnInputDataRecievedSignal>(OnInputRecieved, this);
        signalBus.Subscribe<OnEquipedItemChangedSignal>(OnEquipementChanged, this);
    }

    private void OnEquipementChanged(OnEquipedItemChangedSignal obj)
    {
         foreach (var layer in CombatLayers) _animator.SetLayerWeight(_animator.GetLayerIndex(layer), 0);
    }

    private void Update()
    {
        switch (_inventory.CurrentWeaponType)
        {
            case WeaponType.OneHanded:
                _animator.SetBool("Is onehanded weapon", true);
                _animator.SetBool("Is twohanded weapon", false);
                break;
            case WeaponType.TwoHanded:
                _animator.SetBool("Is twohanded weapon", true);
                _animator.SetBool("Is onehanded weapon", false);
                break;
        }

        if (TargetAttackType != _combatService.CurrentAttackType) return;

        if (_states.States[PlayerState.Attacking])
            _currentAttackProgress += Time.deltaTime;

        if (CurrentAttackNormalizedProgress >= 0.98f)
            OnAttackEnded();
    }

    private void OnAttackEnded()
    {
        if (TargetAttackType != _combatService.CurrentAttackType) return;

        _currentAttack = null;
        if (_nextAttack != null)
        {
            _currentAttack = _nextAttack;
            _nextAttack = null;
            Attack();
        }
        else
        {
            DOVirtual.DelayedCall(0.2f, () =>
            {
                SetCombatLayerActive(false);
            });
            _previousAttack = null;
            _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Attacking, false));
            _animator.SetTrigger("ExitCombat");
        }

        _isDuringTransaction = true;
        DOVirtual.DelayedCall(0.2f, () =>
        {
            _isDuringTransaction = false;
        });
    }

    private void OnInputRecieved(OnInputDataRecievedSignal signal)
    {
        if (TargetAttackType != _combatService.CurrentAttackType) return;

        _lastDirection = signal.Data.Direction;

        if (signal.Data.AttackAttempt)
        {
            if (AttackAvailable)
            {
                if (CurrentAttackNormalizedProgress == 0)
                {
                    Attack();
                    SetCombatLayerActive(true);
                }
                else if ((_currentAttack.Duration - _currentAttackProgress) <= 0.3f && _nextAttack == null)
                {
                    QueueAttack();
                }
            }
        }
    }

    protected virtual void QueueAttack()
    {
        if (TargetAttackType != _combatService.CurrentAttackType) return;

        _nextAttack = _config.GetRandomFirstAttack(_currentAttack.Id, TargetAttackType);
    }

    protected virtual void Attack()
    {
        if (TargetAttackType != _combatService.CurrentAttackType) return;

        _currentAttackProgress = 0;

        _animator.SetTrigger(_currentAttack.Id);
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Attacking, true));

        if (_config.GetAttackById(_currentAttack.Id).PlayerPushForce != Vector3.zero)
        {
            PushPlayer();
        }

        _onEndFight.Kill();
        _onEndFight = null;
    }

    private void PushPlayer()
    {
        if (TargetAttackType != _combatService.CurrentAttackType) return;

        _player.MoveAnim.SetCurve(_currentAttack.PlayerPushCurve);

        Vector3 pushVector;
        if (_lastDirection != Vector2Int.zero)
        {
            pushVector = _player.Model.TransformDirection(new Vector3(0, 0, 1)) * _currentAttack.PlayerPushForce.z
            + _player.Model.TransformDirection(new Vector3(0, 1, 0)) * _currentAttack.PlayerPushForce.y;
        }
        else
        {
            pushVector = _cameraAnchor.transform.TransformDirection(new Vector3(0, 0, 1)) * _currentAttack.PlayerPushForce.z
            + _cameraAnchor.transform.TransformDirection(new Vector3(0, 1, 0)) * _currentAttack.PlayerPushForce.y;
        }
        _player.MoveAnim.SetValues(_player.transform.position, _player.transform.position + pushVector);
        _player.MoveAnim.Play();
    }

    private void SetCombatLayerActive(bool toActive)
    {
        if (TargetAttackType != _combatService.CurrentAttackType) return;
        string layerNameToEnable = CurrentLayerName;
        if (!_states.States[PlayerState.IsArmed])
            layerNameToEnable = "CombatLayerDisarmed";

        for (int i = 0; i < LayersToToggle.Length; i++)
        {
            if (_animator.GetLayerName(_animator.GetLayerIndex(LayersToToggle[i])) != layerNameToEnable)
            {
                if (toActive)
                {
                    _animator.SetLayerWeight(_animator.GetLayerIndex(LayersToToggle[i]), 0);
                    _player.StopAllCoroutines();
                }
                else
                {
                    _animator.SetLayerWeightSmooth(_player,LayersToToggle[i], true, 6);
                }
            }
        }
        for (int i = 0; i < CombatLayers.Length; i++)
        {
            if (_animator.GetLayerName(_animator.GetLayerIndex(LayersToToggle[i])) != layerNameToEnable)
            {
                if (toActive)
                {
                    _animator.SetLayerWeight(_animator.GetLayerIndex(CombatLayers[i]), 0);
                    _player.StopAllCoroutines();
                }
            }
        }
        _animator.SetLayerWeightSmooth(_player, layerNameToEnable, toActive, 6);
    }

    protected void SetCurrentAttack()
    {
        if (TargetAttackType != _combatService.CurrentAttackType) return;

        if (_previousAttack != null)
        {
            _currentAttack = _config.GetRandomFirstAttack(_previousAttack.Id, TargetAttackType);
        }
        else
            _currentAttack = _config.GetRandomFirstAttack("", TargetAttackType);

        _previousAttack = _currentAttack;
    }
}
