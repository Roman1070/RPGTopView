using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

public class GameUiControllerBase
{
    protected SignalBus _signalBus;
    protected GameCanvas _gameCanvas;

    public GameUiControllerBase(SignalBus signalBus, GameCanvas gameCanvas)
    {
        _signalBus = signalBus;
        _gameCanvas = gameCanvas;
    }
}

