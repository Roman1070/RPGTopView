using System.Collections.Generic;

public class GetCharacterStatesSignal : ISignal
{
    public Dictionary<CharacterState, bool> States;

    public GetCharacterStatesSignal(Dictionary<CharacterState, bool> states)
    {
        States = states;
    }
}
