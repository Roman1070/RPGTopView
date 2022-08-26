using UnityEngine;

public class InteractableShrine : InteractableObject
{
    public int ExperienceAmount;
    public ParticleSystem VFX;

    public override string Action => "Pray";

    public override float InteractionTime => 5;
}
