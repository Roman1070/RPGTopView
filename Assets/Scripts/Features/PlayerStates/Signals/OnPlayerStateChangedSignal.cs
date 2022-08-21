public class OnPlayerStateChangedSignal : ISignal
{
    public PlayerState State;
    public bool Value;

    public OnPlayerStateChangedSignal(PlayerState state, bool value)
    {
        State = state;
        Value = value;
    }
}
