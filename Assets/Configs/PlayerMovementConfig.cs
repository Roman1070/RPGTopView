using UnityEngine;

[CreateAssetMenu(fileName = "PlayerMovementConfig" , menuName = "Configs/PlayerMovementConfig")]

public class PlayerMovementConfig : ScriptableObject
{
    public float WalkingSpeed;
    public float RunningSpeed;
    public float WalkingBackSpeed;
    public float Gravity;
    public float GroundCheckDistance;
    public float JumpHeight;
    public float RollDistance;
    public float MaxStamina;
    public float StaminaRegeneration;
    public float StaminaConsuming;
    public float StaminaRegeneratingDelay;
    public float StaminaOnJump;
    public float StaminaOnRoll;

    public AnimationCurve RollCurve;
}

