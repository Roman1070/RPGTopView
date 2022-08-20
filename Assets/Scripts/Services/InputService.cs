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
    public bool CollectAttempt;
    public bool DevConsoleCall;
    public bool InventoryToggle;
}

public class InputService : LoadableService
{
    private float _sens = 2;

    private UpdateProvider _updateProvider;

    public InputService(SignalBus signalBus, UpdateProvider updateProvider) : base(signalBus)
    {
        _updateProvider = updateProvider;
        _updateProvider.Updates.Add(GetInput);
    }

    public KeyCode Right => KeyCode.D;
    public KeyCode Left => KeyCode.A;
    public KeyCode Up => KeyCode.W;
    public KeyCode Down => KeyCode.S;
    public KeyCode Jump => KeyCode.Space;
    public KeyCode Sprint => KeyCode.LeftShift;
    public KeyCode Attack => KeyCode.Mouse0;
    public KeyCode Roll => KeyCode.LeftControl;
    public KeyCode Collect => KeyCode.E;
    public KeyCode DevConsole => KeyCode.KeypadMinus;
    public KeyCode Inventory => KeyCode.I;

    //всю эту движуху сверху в конфиг


    private void GetInput()
    {
        int upperImpact = Input.GetKey(Up) ? 1 : 0;
        int lowerImpact = Input.GetKey(Down) ? 1 : 0;
        int rightImpact = Input.GetKey(Right) ? 1 : 0;
        int leftImpact = Input.GetKey(Left) ? 1 : 0;

        Vector2Int direction = new Vector2Int(rightImpact - leftImpact, upperImpact - lowerImpact);

        float rotX = Input.GetAxis("Mouse X") * _sens;
        float rotY = Input.GetAxis("Mouse Y") * _sens;

        InputDataPack data = new InputDataPack();
        data.Direction = direction;
        data.Rotation = new Vector2(rotX, rotY);
        data.JumpAttempt = Input.GetKeyDown(Jump);
        data.SprintAttempt = Input.GetKeyDown(Sprint);
        data.SprintBreak = Input.GetKeyUp(Sprint);
        data.AttackAttempt = Input.GetKeyDown(Attack);
        data.RollAttempt = Input.GetKeyDown(Roll);
        data.CollectAttempt = Input.GetKeyDown(Collect);
        data.DevConsoleCall = Input.GetKeyDown(DevConsole);
        data.InventoryToggle = Input.GetKeyDown(Inventory);

        _signalBus.FireSignal(new OnInputDataRecievedSignal(data));
    }
}
