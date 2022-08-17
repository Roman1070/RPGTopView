public class OnGroundedStatusChangedSignal : ISignal
{
    public bool IsGrounded;

    public OnGroundedStatusChangedSignal(bool value)
    {
        IsGrounded = value;
    }
}

