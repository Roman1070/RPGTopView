using System;
using UnityEngine;
using Zenject;

public class InputService : LoadableService
{
    [Inject]
    private UpdateProvider _updateProvider;
    [SerializeField]
    private float _sens = 2;

    private KeyCode Right => KeyCode.D;
    private KeyCode Left => KeyCode.A;
    private KeyCode Up => KeyCode.W;
    private KeyCode Down => KeyCode.S;
    private KeyCode Jump => KeyCode.Space;
    private KeyCode Sprint => KeyCode.LeftShift;
    private KeyCode Attack => KeyCode.Mouse0;
    private KeyCode Block => KeyCode.Mouse1;
    //��� ��� ������� ������ � ������

    private bool _movementEnabled;

    public override void Init()
    {
        base.Init();
        _updateProvider.Updates.Add(GetInput);
        _movementEnabled = true;
        _signalBus.Subscribe<OnMovementAbilityStatusChangedSignal>(UpdateMovementAbility,this);
    }

    private void UpdateMovementAbility(OnMovementAbilityStatusChangedSignal signal)
    {
        _movementEnabled = signal.Available;
    }

    private void GetInput()
    {
        int upperImpact = Input.GetKey(Up) ? 1 : 0;
        int lowerImpact = Input.GetKey(Down) ? 1 : 0;
        int rightImpact = Input.GetKey(Right) ? 1 : 0;
        int leftImpact = Input.GetKey(Left) ? 1 : 0;

        Vector2Int direction = new Vector2Int(rightImpact-leftImpact,upperImpact-lowerImpact);

        float rotX = Input.GetAxis("Mouse X") * _sens;
        float rotY = Input.GetAxis("Mouse Y") * _sens;


        InputDataPack data = new InputDataPack();
        data.Direction = direction;
        data.Rotation = new Vector2(rotX, rotY);
        data.JumpAttempt = Input.GetKeyDown(Jump);
        data.SprintAttempt = Input.GetKeyDown(Sprint);
        data.SprintBreak = Input.GetKeyUp(Sprint);
        data.AttackAttempt = Input.GetKeyDown(Attack);

        if (!_movementEnabled)
        {
            data.Direction = Vector2Int.zero;
            data.Rotation = Vector2.zero;
            data.JumpAttempt = false;
        }

        _signalBus.FireSignal(new OnInputDataRecievedSignal(data));
    }
}
