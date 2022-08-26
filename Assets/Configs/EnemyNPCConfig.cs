using System;
using UnityEngine;

public class EnemyNPCConfig : ScriptableObject
{
    public float PatrolingDistance;
    public float PatrolSpeed;
    public float DetectionDistance;
    public float ChaseSpeed;
    public float ChaseDistance;
    public float AttackRange;
    public NPCAttackData[] Attacks;
}

[Serializable]
public class NPCAttackData
{
    public string Id;
    public float Duration;
    public AnimationCurve PushCurve;
    public Vector3 PushForce;
}