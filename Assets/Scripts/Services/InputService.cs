using UnityEngine;

public class InputDataPack
{
    public Vector2Int Direction;
    public Vector2 Rotation;
    public bool JumpAttempt;
    public bool SprintAttempt;
    public bool SprintBreak;
    public bool AttackAttempt;
    public bool RollAttempt;
    public bool DodgeAttempt;
    public bool CollectAttempt;
    public bool DevConsoleCall;
    public bool InventoryCall;
    public bool Esc;
    public bool ToggleArmedStatus;
}

public class InputService : LoadableService
{
    public readonly InputConfig Config;
    private UpdateProvider _updateProvider;
    private CameraMovementConfig _cameraConfig;
    private Vector2 _previousDirection;

    public InputService(SignalBus signalBus, UpdateProvider updateProvider, CameraMovementConfig cameraConfig, InputConfig config) : base(signalBus)
    {
        _updateProvider = updateProvider;
        _cameraConfig = cameraConfig;
        Config = config;
        _updateProvider.Updates.Add(GetInput);
    }

    public override void OnServicesLoaded(params LoadableService[] services)
    {
    }

    private void GetInput()
    {
        int upperImpact = Input.GetKey(Config.Up) ? 1 : 0;
        int lowerImpact = Input.GetKey(Config.Down) ? 1 : 0;
        int rightImpact = Input.GetKey(Config.Right) ? 1 : 0;
        int leftImpact = Input.GetKey(Config.Left) ? 1 : 0;

        Vector2Int direction = new Vector2Int(rightImpact - leftImpact, upperImpact - lowerImpact);

        if (direction != _previousDirection)
        {
            _signalBus.FireSignal(new OnMovementDirectionChagnedSignal(direction));
        }

        float rotX = Input.GetAxis("Mouse X") * _cameraConfig.Sensitivity.x;
        float rotY = Input.GetAxis("Mouse Y") * _cameraConfig.Sensitivity.y;

        InputDataPack data = new InputDataPack();
        data.Direction = direction;
        data.Rotation = new Vector2(rotX, rotY);
        data.JumpAttempt = Input.GetKeyDown(Config.Jump);
        data.SprintAttempt = Input.GetKeyDown(Config.Sprint);
        data.SprintBreak = Input.GetKeyUp(Config.Sprint);
        data.AttackAttempt = Input.GetKeyDown(Config.Attack);
        data.RollAttempt = Input.GetKeyDown(Config.Roll);
        data.CollectAttempt = Input.GetKeyDown(Config.Collect);
        data.DevConsoleCall = Input.GetKeyDown(Config.DevConsole);
        data.InventoryCall = Input.GetKeyDown(Config.Inventory);
        data.Esc = Input.GetKeyDown(KeyCode.Escape);
        data.ToggleArmedStatus = Input.GetKeyDown(Config.ToggleArmedStatus);
        data.DodgeAttempt = Input.GetKeyDown(Config.Dodge);

        _previousDirection = direction;
        _signalBus.FireSignal(new OnInputDataRecievedSignal(data));

    }
}
