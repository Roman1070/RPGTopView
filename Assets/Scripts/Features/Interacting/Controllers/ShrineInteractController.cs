public class ShrineInteractController : ItemInteractControllerBase
{
    public ShrineInteractController(SignalBus signalBus, PlayerView player) : base(signalBus,player)
    {
    }

    protected override void CheckObjectType(InteractableObject obj)
    {
        if (obj is InteractableShrine)
        {
            obj = obj as InteractableShrine;
            _player.StartCoroutine(Interact(obj));
        }
    }

    protected override void OnCollected(InteractableObject obj)
    {
        _signalBus.FireSignal(new OnExperienceChangedSignal((obj as InteractableShrine).ExperienceAmount));
    }
}
