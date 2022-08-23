public class PlayerThoHandedWeaponCombatController : PlayerCombatControllerBase
{
    protected override string CurrentLayerName => "CombatLayerTwoHanded";
    protected override WeaponType TargetWeaponType => WeaponType.TwoHanded;

    public PlayerThoHandedWeaponCombatController(SignalBus signalBus, PlayerView player,  PlayerCombatConfig config, PlayerStatesService states, UpdateProvider updateProvider, PlayerCombatService combatService)
        : base(signalBus, player, config, states, updateProvider,combatService)
    {
    }

    protected override void Attack()
    {
        if (_currentAttack == null)
        {
            if (_animator.GetInteger("Speed") >= 0)
                SetCurrentAttack();
            else
            {
                _currentAttack = _config.GetAttackById("WalkingBackAttack");
                _previousAttack = null;
            }
        }

        base.Attack();
    }

    protected override void QueueAttack()
    {
        if (_currentAttack.Id == "Combo1")
            _nextAttack = _config.GetAttackById("Combo2");
        else if (_currentAttack.Id == "Combo2")
            _nextAttack = _config.GetAttackById("Combo3");
        else
            _nextAttack = _config.GetRandomFirstAttack(_currentAttack.Id,TargetWeaponType);
    }

}
