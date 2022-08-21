using DG.Tweening;
using System;
using UnityEngine;

public class PlayerStaminaController : PlayerMovementControllerBase
{
    private float _stamina;
    private bool _staminaIsGrowing;
    private PlayerMovementConfig _config;

    private Tween _StaminaGrowthEnabler;

    public PlayerStaminaController(PlayerView player, SignalBus signalBus, PlayerMovementConfig config, UpdateProvider updateProvider, PlayerStatesService playerStatesService) : base(player, signalBus, playerStatesService)
    {
        _config = config;

        _signalBus.Subscribe<OnPlayerStateChangedSignal>(OnPlayerStateChagned, this);
        _signalBus.Subscribe<OnStaminaChangedSignal>(OnStaminaChaged, this);
        updateProvider.Updates.Add(UpdateStamina);

        _stamina = _config.MaxStamina;
    }

    private void OnPlayerStateChagned(OnPlayerStateChangedSignal obj)
    {
        if (obj.State == PlayerState.Running)
        {
            if(obj.Value)
            {
                _StaminaGrowthEnabler.Kill();
                _staminaIsGrowing = false;
            }
            else
            {
                TryRestoreStamina();
            }
        }
    }

    private void OnStaminaChaged(OnStaminaChangedSignal signal)
    {
        if(signal.Stamina < _stamina)
        {
            TryRestoreStamina();
        }
        _stamina = signal.Stamina;
    }

    private void TryRestoreStamina()
    {
        _StaminaGrowthEnabler.Kill();
        _staminaIsGrowing = false;
        _StaminaGrowthEnabler = DOVirtual.DelayedCall(_config.StaminaRegeneratingDelay, () =>
        {
            _staminaIsGrowing = true;
        });
    }

    private void UpdateStamina()
    {
        if (_states.States[PlayerState.Running])
        {
            _stamina -= _config.StaminaConsuming * Time.deltaTime;
        }
        else if (_staminaIsGrowing)
        {
            _stamina += _config.StaminaRegeneration * Time.deltaTime;
            _stamina = Mathf.Clamp(_stamina, 0, _config.MaxStamina);
        }

        _signalBus.FireSignal(new OnStaminaChangedSignal(_stamina));
    }
}
