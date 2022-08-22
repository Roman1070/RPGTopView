using DG.Tweening;
using System.Collections;
using UnityEngine;

public class PlayerCombatController : PlayerCombatControllerBase
{
    private PlayerStatesService _states;
    private Animator _animator;
    private AttackData _currentAttack;
    private AttackData _previousAttack;
    private AttackData _nextAttack;
    private UpdateProvider _updateProvider;
    private float _currentAttackProgress;
    private Tween _onEndFight;

    private float CurrentAttackNormalizedProgress
    {
        get
        {
            if (_currentAttack == null) return 0;

            return _currentAttackProgress / _currentAttack.Duration;
        }
    }

    public PlayerCombatController(PlayerView player, SignalBus signalBus, PlayerCombatConfig config, PlayerStatesService states, UpdateProvider updateProvider) : base(signalBus, player, config)
    {
        _animator = player.Model.GetComponent<Animator>();
        _config = config;
        _states = states;
        _updateProvider = updateProvider;
        _updateProvider.Updates.Add(Update);
        _animator.SetLayerWeight(_animator.GetLayerIndex("CombatLayerFullBody"), 0);
        signalBus.Subscribe<OnInputDataRecievedSignal>(OnInputRecieved, this);
    }

    private void Update()
    {
        if (_states.States[PlayerState.Attacking])
        {
            _currentAttackProgress += Time.deltaTime;
        }

        if (CurrentAttackNormalizedProgress >= 0.98f)
        {
            OnAttackEnded();
        }
    }

    private void OnAttackEnded()
    {
        _currentAttack = null;
        if (_nextAttack != null)
        {
            _currentAttack = _nextAttack;
            _nextAttack = null;
            Attack();
        }
        else
        {
            SetCombatLayerActive(false);
            _previousAttack = null;
            _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Attacking, false));
            _animator.SetTrigger("ExitCombat");
        }
    }

    private void OnInputRecieved(OnInputDataRecievedSignal signal)
    {
        if (signal.Data.AttackAttempt)
        {
            bool attackAvaialbe = !(!_states.States[PlayerState.Grounded] || _states.States[PlayerState.Rolling] || _states.States[PlayerState.DrawingWeapon]);

            if (attackAvaialbe)
            {
                if (!_states.States[PlayerState.IsArmed])
                {
                    _signalBus.FireSignal(new DrawWeaponSignal(true));
                    return;
                }
                if (CurrentAttackNormalizedProgress == 0)
                {
                    Attack();
                    SetCombatLayerActive(true);
                }
                else if (CurrentAttackNormalizedProgress >= 0.5f)
                    QueueAttack();
            }
        }
    }

    private void QueueAttack()
    {
        if (_nextAttack != null) return;

        if (_currentAttack.Id == "Combo1")
            _nextAttack = _config.GetDataById("Combo2");
        else if (_currentAttack.Id == "Combo2")
            _nextAttack = _config.GetDataById("Combo3");
        else
            _nextAttack = _config.GetRandomFirstAttack(_currentAttack.Id);
    }

    private void Attack()
    {
        _currentAttackProgress = 0;
        if (_currentAttack == null) SetCurrentAttack();

        _animator.SetBool(_currentAttack.Id, true);
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Attacking, true));

        _onEndFight.Kill();
        _onEndFight = null;
    }

    private void SetCombatLayerActive(bool toActive)
    {
        for (int i = 0; i < _animator.layerCount; i++)
        {
            if(_animator.GetLayerName(i)!= "CombatLayerFullBody")
            {
                if (toActive)
                {
                    _animator.SetLayerWeight(i, 0);
                }
                else
                {
                    _player.StartCoroutine(SetLayerWeightSmmoth(i, true));
                }
            }
        }
        _player.StartCoroutine(SetLayerWeightSmmoth(_animator.GetLayerIndex("CombatLayerFullBody"), toActive));
    }

    private IEnumerator SetLayerWeightSmmoth(int layer, bool turnActive)
    {
        if (turnActive)
        {
            float w = 0;
            while (w < 1)
            {
                yield return new WaitForEndOfFrame();
                w += Time.deltaTime*4;
                _animator.SetLayerWeight(layer, w);
            }
        }
        else
        {
            float w = 1;
            while (w >0)
            {
                yield return new WaitForEndOfFrame();
                w -= Time.deltaTime*4;
                _animator.SetLayerWeight(layer, w);
            }
        }
    }

    private void SetCurrentAttack()
    {
        if (_previousAttack != null)
        {
            _currentAttack = _config.GetRandomFirstAttack(_previousAttack.Id);
        }
        else
            _currentAttack = _config.GetRandomFirstAttack("");

        _previousAttack = _currentAttack;
    }
}
