public class SetCharacterStateSignal : ISignal
{
    public CharacterState State;
    public bool Value;

    public SetCharacterStateSignal(CharacterState state, bool value)
    {
        State = state;
        Value = value;
    }
}
