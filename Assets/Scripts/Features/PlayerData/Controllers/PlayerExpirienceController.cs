using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerExpirienceController : PlayerDataControllerBase
{
    public int Expirience { get; private set; }
    public int Level { get; private set; }
    private int CurrentLevelExpirience => Expirience - CurrentLevelRequiredExp;
    private int CurrentLevelRequiredExp => _levelsConfig.ExpOnLevel[Level - 1];
    private int NextLevelRequiredExp => _levelsConfig.ExpOnLevel[Level];

    private float NormalizedExp => Mathf.Clamp((float)CurrentLevelExpirience / (NextLevelRequiredExp - CurrentLevelRequiredExp),0,1);

    private PlayerLevelsConfig _levelsConfig;

    public PlayerExpirienceController(SignalBus signalBus, PlayerLevelsConfig levelConfig) : base(signalBus)
    {
        _levelsConfig = levelConfig;

        if (Level == 0)
        {
            Expirience = 0;
            Level = 1;
        }

        signalBus.Subscribe<OnExperienceChangedSignal>(OnExpChanged,this);
        DOVirtual.DelayedCall(0.1f, () => { signalBus.FireSignal(new UpdatePlayerUiWidgetSignal(NormalizedExp, Level, false)); } );
    }

    private void OnExpChanged(OnExperienceChangedSignal signal)
    {
        Expirience += signal.Value;

        UpdateLevel();
    }

    private void UpdateLevel()
    {
        var queue = new Queue<UpdatePlayerUiWidgetSignal>();

        queue.Enqueue(new UpdatePlayerUiWidgetSignal(NormalizedExp, Level,false));

        for (int i = Level; i < _levelsConfig.ExpOnLevel.Length;i++)
        {
            if (Expirience >= _levelsConfig.ExpOnLevel[i])
            {
                Level = i + 1;
                _signalBus.FireSignal(new OnPlayerLevelIncreasedSignal());
                queue.Enqueue(new UpdatePlayerUiWidgetSignal(NormalizedExp, Level,true));

            }
        }
        _signalBus.FireSignal(new QueueUpdatePlayerWidgetSignals(queue));
    }
}

