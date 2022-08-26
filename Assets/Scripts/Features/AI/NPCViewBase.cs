using UnityEngine;
using UnityEngine.AI;

public enum NPCType
{
    Enemy,
    Ally,
    Neutral
}

public enum NPCState
{
    Idle,
    Patroling,
    Chasing,
    Attacking,
    Speaking,
    Escaping,
}

public abstract class NPCViewBase : View
{
    protected abstract NPCType NPCType { get; }
}
