using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "PlayerCombatConfig", menuName = "Configs/PlayerCombatConfig")]
public class PlayerCombatConfig : ScriptableObject
{
    public float[] AttacksDurations;
}
