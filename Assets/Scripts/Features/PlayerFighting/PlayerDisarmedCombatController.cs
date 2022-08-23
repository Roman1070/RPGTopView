public class PlayerDisarmedCombatController : PlayerCombatControllerBase
{
    protected override string CurrentLayerName => "CombatLayerDisarmed";
    protected override WeaponType TargetWeaponType => WeaponType.Disarmed;

    public PlayerDisarmedCombatController(SignalBus signalBus, PlayerView player, PlayerCombatConfig config, PlayerStatesService states, UpdateProvider updateProvider, PlayerCombatService combatService) 
        : base(signalBus, player, config, states, updateProvider,combatService)
    {
    }

    protected override void QueueAttack()
    {
        _nextAttack = _config.GetRandomFirstAttack(_currentAttack.Id, TargetWeaponType);
    }

    protected override void Attack()
    {
        if (_currentAttack == null)
        {
            SetCurrentAttack();
        }
        base.Attack();
    }
}
