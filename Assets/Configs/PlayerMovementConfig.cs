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
    public float MaxStamina;
    public float StaminaRegeneration;
    public float StaminaConsuming;
    public float StaminaRegeneratingDelay;
    public float StaminaOnJump;
}

