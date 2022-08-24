public class PlayerDisarmedCombatController : PlayerCombatControllerBase
{
    protected override string CurrentLayerName => "CombatLayerDisarmed";
    protected override AttackType TargetAttackType => AttackType.Disarmed;

    public PlayerDisarmedCombatController(SignalBus signalBus, PlayerView player, PlayerCombatConfig config,
        PlayerStatesService states, UpdateProvider updateProvider, PlayerCombatService combatService, Inventory inventory) 
        : base(signalBus, player, config, states, updateProvider,combatService, inventory)
    {
    }

    protected override void Attack()
    {
        if (_currentAttack == null)
        {
            if (_animator.GetFloat("Speed") >=1.8f)
            {
                _currentAttack = _config.GetAttackById("Double jump kick");
            }
            else
            {
                SetCurrentAttack();
            }
        }
        base.Attack();
    }
}
