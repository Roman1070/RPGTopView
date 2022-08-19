using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class StaminaSliderController : GameUiControllerBase
{
    private GameCanvas _canvas;
    private Slider _slider;
    private GroupOpacityAnimation _sliderAnim;
    private float _maxStamina;

    private Tween _hideSlider;

    public StaminaSliderController(SignalBus signalBus, GameCanvas gameCanvas, PlayerMovementConfig movementConfig) : base(signalBus, gameCanvas)
    {
        _canvas = gameCanvas;
        _slider = _canvas.GetView<GameUiPanel>().StaminaSlider;
        _sliderAnim = _slider.GetComponent<GroupOpacityAnimation>();
        _maxStamina = movementConfig.MaxStamina;
        _slider.maxValue = _maxStamina;
        //_sliderAnim.SetProgress(1);

        _hideSlider = DOVirtual.DelayedCall(0,()=> { });

        signalBus.Subscribe<OnStaminaChangedSignal>(UpdateSlider, this);
    }

    private void UpdateSlider(OnStaminaChangedSignal signal)
    {
        _slider.value = signal.Stamina;
        if (signal.Stamina >= _maxStamina - 1)
        {
            if (_hideSlider == null)
            {
                _hideSlider = DOVirtual.DelayedCall(1, () =>
                {
                    _sliderAnim.Play();
                });
            }
        }
        else
        {
            _hideSlider.Kill();
            _hideSlider = null;
            _sliderAnim.SetProgress(0);
        }
    }
}

