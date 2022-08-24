public class PlayerTwoHandedWeaponCombatController : PlayerCombatControllerBase
{
    protected override string CurrentLayerName => "CombatLayerTwoHanded";
    protected override AttackType TargetAttackType => AttackType.TwoHanded;

    public PlayerTwoHandedWeaponCombatController(SignalBus signalBus, PlayerView player, PlayerCombatConfig config, PlayerStatesService states,
        UpdateProvider updateProvider, PlayerCombatService combatService, Inventory inventory)
        : base(signalBus, player, config, states, updateProvider, combatService, inventory)
    {
    }

    protected override void Attack()
    {
        if (_currentAttack == null)
        {
            if (_animator.GetFloat("Speed") >= 0)
                SetCurrentAttack();
            else
            {
                _currentAttack = _config.GetAttackById("WalkingBackAttack");
                _previousAttack = null;
            }
        }
        base.Attack();
    }
}
