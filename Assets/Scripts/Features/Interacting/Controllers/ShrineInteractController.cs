using DG.Tweening;

public class ShrineInteractController : ItemInteractControllerBase
{
    private PlayerStatesService _statesService;
    public ShrineInteractController(SignalBus signalBus, PlayerView player, PlayerStatesService statesService) : base(signalBus, player)
    {
        _statesService = statesService;
    }

    protected override void Interact(InteractableObject obj)
    {
        if (obj is InteractableShrine)
        {
            if (_statesService.States[PlayerState.IsArmed])
            {
                _signalBus.FireSignal(new DrawWeaponSignal(false));
                DOVirtual.DelayedCall(1, () =>
                {
                    Pray(obj as InteractableShrine);
                });
            }
            else
            {
                Pray(obj as InteractableShrine);
            }
        }
    }

    private void Pray(InteractableShrine obj)
    {
        _animator.SetTrigger("Pray");
        _player.StartCoroutine(InteractProcess(obj));
    }

    protected override void OnCollected(InteractableObject obj)
    {
        _signalBus.FireSignal(new OnExperienceChangedSignal((obj as InteractableShrine).ExperienceAmount));
    }
}
