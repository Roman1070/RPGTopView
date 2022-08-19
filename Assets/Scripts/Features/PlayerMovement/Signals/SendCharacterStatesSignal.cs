using System.Collections.Generic;

public class SendCharacterStatesSignal : ISignal
{
    public Dictionary<PlayerState, bool> States;

    public SendCharacterStatesSignal(Dictionary<PlayerState, bool> states)
    {
        States = states;
    }
}
