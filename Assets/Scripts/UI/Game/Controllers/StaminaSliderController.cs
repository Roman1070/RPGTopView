using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class StaminaSliderController : GameUiControllerBase
{
    public StaminaSliderController(SignalBus signalBus) : base(signalBus)
    {
        signalBus.Subscribe<OnStaminaChangedSignal>(UpdateSlider, this);
    }

    private void UpdateSlider(OnStaminaChangedSignal obj)
    {

    }
}

