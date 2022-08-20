using UnityEngine;

public class DevConsoleService : LoadableService
{
    private object[] _args;
    private bool _consoleIsAvtive;
    private DevConsoleView _console;
    private CanvasGroup _canvasGroup;

    public DevConsoleService(SignalBus signalBus, GameCanvas gameCanvas) : base(signalBus)
    {
        signalBus.Subscribe<OnInputDataRecievedSignal>(OnInput, this);
        _console = gameCanvas.GetView<GameUiPanel>().GetView<DevConsoleView>();
        _canvasGroup = _console.GetComponent<CanvasGroup>();
        SetConsoleActive(false);
        InitButton();
    }

    private void OnInput(OnInputDataRecievedSignal obj)
    {
        if (obj.Data.DevConsoleCall)
        {
            _consoleIsAvtive = !_consoleIsAvtive;
            SetConsoleActive(_consoleIsAvtive);
        }
    }

    private void SetConsoleActive(bool val)
    {
        Cursor.lockState = val ? CursorLockMode.None : CursorLockMode.Locked;
        if (val) _console.InputField.text = "";
        _canvasGroup.alpha = val ? 1 : 0;
        _canvasGroup.blocksRaycasts = val;
        _signalBus.FireSignal(new SetPlayerStateSignal(PlayerState.Collecting, val));
    }

    private void InitButton()
    {
        _console.Button.onClick.RemoveAllListeners();
        _console.Button.onClick.AddListener(() => { TrySendSignal(); });
    }

    private void TrySendSignal()
    {
        string[] parts = _console.InputField.text.Split(' ');

        string command = parts[0];
        _args = new object[parts.Length - 1];
        for (int i = 0; i < _args.Length; i++)
        {
            _args[i] = int.Parse(parts[i + 1]);
        }

        _console.InputField.text = "";

        switch (command)
        {
            case "addexp":
                _signalBus.FireSignal(new OnExperienceChangedSignal((int)_args[0]));
                break;
        }

    }

}
