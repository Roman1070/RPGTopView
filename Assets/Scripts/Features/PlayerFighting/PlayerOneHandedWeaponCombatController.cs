public class PlayerOneHandedWeaponCombatController : PlayerCombatControllerBase
{
    protected override string CurrentLayerName => "CombatLayerOneHanded";
    protected override AttackType TargetAttackType => AttackType.OneHanded;

    public PlayerOneHandedWeaponCombatController(SignalBus signalBus, PlayerView player, PlayerCombatConfig config, PlayerStatesService states, UpdateProvider updateProvider
        , PlayerCombatService combatService, Inventory inventory)
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

    protected override void QueueAttack()
    {
        if (_currentAttack.Id == "Combo1")
            _nextAttack = _config.GetAttackById("Combo2");
        else if (_currentAttack.Id == "Combo2")
            _nextAttack = _config.GetAttackById("Combo3");
        else if (_currentAttack.Id == "WalkingBackAttack")
            _nextAttack = _animator.GetFloat("BlendSpeed") >= 0 ? _config.GetRandomFirstAttack(_currentAttack.Id, TargetAttackType) : _config.GetAttackById("WalkingBackAttack");
        else
            _nextAttack = _config.GetRandomFirstAttack(_currentAttack.Id, TargetAttackType);
    }
}
