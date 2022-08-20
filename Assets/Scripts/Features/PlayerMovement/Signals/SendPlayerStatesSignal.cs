using System.Collections.Generic;

public class SendPlayerStatesSignal : ISignal
{
    public Dictionary<PlayerState, bool> States;

    public SendPlayerStatesSignal(Dictionary<PlayerState, bool> states)
    {
        States = states;
    }
}
