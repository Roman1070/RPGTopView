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

    public override void Init()
    {
        base.Init();
        _updateProvider.Updates.Add(GetInput);
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
        _signalBus.FireSignal(new OnInputDataRecievedSignal(direction,new Vector2(rotX,rotY),Input.GetKeyDown(Jump),
            Input.GetKeyDown(Sprint),Input.GetKeyUp(Sprint)));
    }
}
