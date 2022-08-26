public class PlayerTwoHandedWeaponCombatController : PlayerCombatControllerBase
{
    public PlayerTwoHandedWeaponCombatController(SignalBus signalBus, PlayerView player, PlayerCombatConfig config,
        PlayerStatesService states, UpdateProvider updateProvider, PlayerCombatService combatService, Inventory inventory, MainCameraAnchor cameraAnchor)
        : base(signalBus, player, config, states, updateProvider, combatService, inventory, cameraAnchor)
    {
    }

    protected override string CurrentLayerName => "CombatLayerTwoHanded";
    protected override AttackType TargetAttackType => AttackType.TwoHanded;

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
        if (_currentAttack.Id == "WalkingBackAttack")
            _nextAttack = _animator.GetFloat("BlendSpeed") >= 0 ? _config.GetRandomFirstAttack(_currentAttack.Id, TargetAttackType) : _config.GetAttackById("WalkingBackAttack");
        else _nextAttack = _config.GetRandomFirstAttack(_currentAttack.Id, TargetAttackType);
    }
}
