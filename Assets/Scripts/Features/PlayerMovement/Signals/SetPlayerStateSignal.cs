public class SetPlayerStateSignal : ISignal
{
    public PlayerState State;
    public bool Value;

    public SetPlayerStateSignal(PlayerState state, bool value)
    {
        State = state;
        Value = value;
    }
}
