using Godot;

public partial class PlayerMeleeAttack : Node
{
    [Export] private InputActions _inputActions;
    [Export] private PlayerPropInteractor _playerObjectInteractor;
    [Export] private RayCast3D _rayOfAttack;
    [Export] private Timer _attackCooldownTimer;
    [Export] private Timer _hardAttackChargeTimer;
    [Export] private float _attackCooldownTime;
    [Export] private float _hardAttackChargeTimeMax;
    [Export] private int _hardAttackMinDamage;
    [Export] private int _hardAttackMaxDamage;
    [Export] private int _lightAttackDamage;

    private bool _isCooldownAttackTimeout = true;

    public override void _Ready()
    {
        _hardAttackChargeTimer.OneShot = true;
        _hardAttackChargeTimer.WaitTime = _hardAttackChargeTimeMax;

        _attackCooldownTimer.WaitTime = _attackCooldownTime;

        _attackCooldownTimer.Timeout += CooldownAttackTimeout;
        _inputActions.AlternativeKeyDown += StartChargeHardAttack;
        _inputActions.AlternativeKeyUp += () => AttemptAttack(CalculateHardAttackDamage());
        _inputActions.AttackKeyDown += () => AttemptAttack(_lightAttackDamage);
    }

    public override void _ExitTree()
    {
        _attackCooldownTimer.Timeout -= CooldownAttackTimeout;
        _inputActions.AlternativeKeyDown -= StartChargeHardAttack;
        _inputActions.AlternativeKeyUp -= () => AttemptAttack(CalculateHardAttackDamage());
        _inputActions.AttackKeyDown -= () => AttemptAttack(_lightAttackDamage);
    }

    private void CooldownAttackTimeout()
    {
        _isCooldownAttackTimeout = true;
    }

    private void StartChargeHardAttack()
    {
        if (_playerObjectInteractor.DraggedObject != null && _isCooldownAttackTimeout)
            _hardAttackChargeTimer.Start();
    }

    private int CalculateHardAttackDamage()
    {
        float chargePercentage = 1.0f - ((float)_hardAttackChargeTimer.TimeLeft / (float)_hardAttackChargeTimer.WaitTime);
        int damage = (int)Mathf.Lerp(_hardAttackMinDamage, _hardAttackMaxDamage, chargePercentage);

        _hardAttackChargeTimer.Stop();

        return damage;
    }

    private void AttemptAttack(int damage)
    {
        if (!_isCooldownAttackTimeout || _playerObjectInteractor.DraggedObject != null)
            return;

        PerformAttack(damage);
    }

    private void PerformAttack(int damage)
    {
        Node collider = _rayOfAttack.GetCollider() as Node;

        _isCooldownAttackTimeout = false;
        _attackCooldownTimer.Start();

        if (!IsLookingAtPlayer())
            return;

        PlayerHealth playerHealth = GetPlayerHealth(collider);
        playerHealth.DecreaseHealth(damage);
    }

    private bool IsLookingAtPlayer()
    {
        Node collider = _rayOfAttack.GetCollider() as Node;
        if (collider == null)
            return false;

        return collider.IsInGroup("Hitbox:Player");
    }

    private PlayerHealth GetPlayerHealth(Node collider)
    {
        Node playerRoot = collider.GetNode(collider.GetMeta("PlayerRoot").ToString());
        return playerRoot?.GetNode(playerRoot.GetMeta("PlayerHealth").ToString()) as PlayerHealth;
    }
}
