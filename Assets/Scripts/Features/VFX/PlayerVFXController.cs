using System.Collections.Generic;
using UnityEngine;

public class PlayerVFXController : VFXControllerBase
{
    private PlayerView _player;

    private string LevelUpVFXPath => "VFX/LevelUpVFX";
    private Dictionary<VFXType, ParticleSystem> _cachedVFXs;

    public PlayerVFXController(SignalBus signalBus, PlayerView player) : base(signalBus)
    {
        _player = player;

        _cachedVFXs = new Dictionary<VFXType, ParticleSystem>()
        {
            {VFXType.LevelUp, null}
        };
        signalBus.Subscribe<OnPlayerLevelIncreasedSignal>(OnLevelIncreased, this);
    }

    private void OnLevelIncreased(OnPlayerLevelIncreasedSignal obj)
    {
        if(_cachedVFXs[VFXType.LevelUp] == null)
        {
            var effect = GameObject.Instantiate(Resources.Load<ParticleSystem>(LevelUpVFXPath)).GetComponent<ParticleSystem>();
            effect.transform.parent = _player.transform;
            effect.transform.localPosition = Vector3.zero;
            effect.Play();
            _cachedVFXs[VFXType.LevelUp] = effect;
        }
        else
        {
            _cachedVFXs[VFXType.LevelUp].Stop();
            _cachedVFXs[VFXType.LevelUp].Play();
        }
    }
}
