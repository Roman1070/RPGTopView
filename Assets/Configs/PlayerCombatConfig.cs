using System;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCombatConfig", menuName = "Configs/PlayerCombatConfig")]
public class PlayerCombatConfig : ScriptableObject
{
    public AttackData[] Attacks;

    public AttackData GetDataById(string id) => Attacks.First(a => a.Id == id);

    public AttackData GetRandomFirstAttack(string expceptId)
    {
        var attacks = Attacks.Where(a => a.Id != "Combo2" && a.Id != "Combo3" && a.Id != expceptId && a.Id != "WalkingBackAttack").ToArray();
        return attacks[UnityEngine.Random.Range(0, attacks.Length)];
    }
}

[Serializable]
public class AttackData
{
    public string Id;
    public float Duration;
    public float DamageMultiplier;
    public AnimationCurve PlayerPushCurve;
    public Vector3 PlayerPushForce;
}