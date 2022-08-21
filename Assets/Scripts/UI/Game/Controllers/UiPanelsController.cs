using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class UiPanelsController : GameUiControllerBase
{
    private List<Panel> _panels;
    private Type _activePanelType;
    private Type _previousPanelType;

    public UiPanelsController(SignalBus signalBus, GameCanvas gameCanvas) : base(signalBus, gameCanvas)
    {
        _panels = gameCanvas.GetViews<Panel>();
        _activePanelType = gameCanvas.GetView<GameUiPanel>().GetType();
        _signalBus.Subscribe<SetActivePanelSignal>(SetActivePanel, this);
        _signalBus.Subscribe<BackToPreviuosPanelSignal>(BackToPreviousPanel, this);
    }

    private void SetActivePanel(SetActivePanelSignal obj)
    {
        var panelToActivate = _panels.First(p => p.GetType() == obj.PanelType);

        if (panelToActivate.IsActive) return;

        foreach (var panel in _panels)
        {
            panel.SetActive(false);
        }

        _previousPanelType = _activePanelType;
        panelToActivate.SetActive(true);
        _activePanelType = panelToActivate.GetType();
    }

    private void BackToPreviousPanel(BackToPreviuosPanelSignal signal)
    {
        if(_activePanelType!=typeof(GameUiPanel))
            SetActivePanel(new SetActivePanelSignal(_previousPanelType));
    }
}
