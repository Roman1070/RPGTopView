using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCombatConfig", menuName = "Configs/PlayerCombatConfig")]
public class PlayerCombatConfig : ScriptableObject
{
    public AttackData[] Attacks;

    public AttackData GetAttackById(string id) => Attacks.First(a => a.Id == id);

    public AttackData GetRandomFirstAttack(string expceptId, AttackType targetAttackType)
    {
        var attacks = Attacks.Where(a => a.InitialAttack && a.Id != expceptId && a.TargetAttackType==targetAttackType).ToArray();
        return attacks[UnityEngine.Random.Range(0, attacks.Length)];
    }
}

[Serializable]
public class AttackData
{
    public string Id;
    public AttackType TargetAttackType;
    public bool InitialAttack;
    public float Duration;
    public float DamageMultiplier;
    public AnimationCurve PlayerPushCurve;
    public Vector3 PlayerPushForce;
}