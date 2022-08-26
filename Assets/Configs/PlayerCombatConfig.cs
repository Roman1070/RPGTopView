using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCombatConfig", menuName = "Configs/PlayerCombatConfig")]
public class PlayerCombatConfig : ScriptableObject
{
    public PlayerAttackData[] Attacks;

    public PlayerAttackData GetAttackById(string id) => Attacks.First(a => a.Id == id);

    public PlayerAttackData GetRandomFirstAttack(string expceptId, AttackType targetAttackType)
    {
        var attacks = Attacks.Where(a => a.InitialAttack && a.Id != expceptId && a.TargetAttackType==targetAttackType).ToArray();
        return attacks[UnityEngine.Random.Range(0, attacks.Length)];
    }
}

[Serializable]
public class PlayerAttackData
{
    public string Id;
    public AttackType TargetAttackType;
    public bool InitialAttack;
    public float Duration;
    public float DamageMultiplier;
    public AnimationCurve PlayerPushCurve;
    public Vector3 PlayerPushForce;
}