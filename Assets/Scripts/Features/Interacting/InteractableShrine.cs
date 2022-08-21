public class InteractableShrine : InteractableObject
{
    public int ExperienceAmount;

    public override string Action => "Pray";

    public override float InteractionTime => 5;
}
