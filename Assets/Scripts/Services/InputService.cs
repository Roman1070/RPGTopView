using UnityEngine;

public struct InputDataPack
{
    public Vector2Int Direction;
    public Vector2 Rotation;
    public bool JumpAttempt;
    public bool SprintAttempt;
    public bool SprintBreak;
    public bool AttackAttempt;
    public bool RollAttempt;
    public bool CollectAttempt;
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

    private KeyCode Right => KeyCode.D;
    private KeyCode Left => KeyCode.A;
    private KeyCode Up => KeyCode.W;
    private KeyCode Down => KeyCode.S;
    private KeyCode Jump => KeyCode.Space;
    private KeyCode Sprint => KeyCode.LeftShift;
    private KeyCode Attack => KeyCode.Mouse0;
    private KeyCode Roll => KeyCode.LeftControl;
    private KeyCode Collect => KeyCode.E;
    private KeyCode Block => KeyCode.Mouse1;
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

        _signalBus.FireSignal(new OnInputDataRecievedSignal(data));
    }
}
