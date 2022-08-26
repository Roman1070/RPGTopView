using DG.Tweening;
using UnityEngine.UI;

public class LevelUpAnimationView : View
{
    public GroupOpacityAnimation ContainerFade;
    public ScaleAnimation LevelUpScaleAnim;
    public ColorAnimation LevelUpColorAnim;
    public TransformAnimation EffectTransform;
    public ColorAnimation EffectColor;
    public OpacityAnimation MiddleRomb;
    public ScaleAnimation Spacer;
    public bool IsPlaying;
    private Tween _isPlayingTween;

    private void Start()
    {
        ContainerFade.SetProgress(1);
    }

    public void Animate()
    {
        ResetValues();
        IsPlaying = true;
        float delay = 0;
        LevelUpScaleAnim.Play(delay);
        LevelUpColorAnim.Play(delay);
        delay += LevelUpScaleAnim.Duration+0.5f;
        MiddleRomb.Play(delay);
        delay += MiddleRomb.Duration;
        Spacer.Play(delay);
        delay += 0.5f;
        DOVirtual.DelayedCall(delay, () => EffectColor.Graphic.SetAlpha(1));
        EffectColor.Play(delay);
        EffectTransform.Play(delay);
        delay += 1;
        ContainerFade.Play(delay);

        _isPlayingTween = null;
        _isPlayingTween = DOVirtual.DelayedCall(delay, () => IsPlaying = false);
    }

    private void ResetValues()
    {
        ContainerFade.SetProgress(0);
        LevelUpColorAnim.SetProgress(0);
        LevelUpScaleAnim.SetProgress(0);
        EffectTransform.SetProgress(0);
        EffectColor.SetProgress(0);
        EffectColor.Graphic.SetAlpha(0);
        MiddleRomb.SetProgress(0);
        Spacer.SetProgress(0);
    }
}
