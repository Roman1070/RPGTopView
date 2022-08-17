using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameUiControllerBase
{
    protected SignalBus _signalBus;

    public GameUiControllerBase(SignalBus signalBus)
    {
        _signalBus = signalBus;
    }
}

