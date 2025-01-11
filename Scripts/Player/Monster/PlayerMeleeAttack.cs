using Godot;

public partial class PlayerMeleeAttack : Node
{
    [Export] private InputActions _inputActions;
    [Export] private PlayerPropInteractor _playerObjectInteractor;
    [Export] private RayCast3D _rayOfAttack;
    [Export] private Timer _attackCooldownTimer;
    [Export] private Timer _hardAttackChargeTimer;
    [Export] private AudioPlayer _audioPlayer;
    [Export] private PlayerMeleeAttackParameters _meleeAttackParameters;

    private bool _isCooldownAttackTimeout = true;

    public override void _Ready()
    {
        _hardAttackChargeTimer.OneShot = true;
        _hardAttackChargeTimer.WaitTime = _meleeAttackParameters.HardAttackChargeTimeMax;

        _attackCooldownTimer.WaitTime = _meleeAttackParameters.AttackCooldownTime;

        _attackCooldownTimer.Timeout += CooldownAttackTimeout;
        _inputActions.AlternativeKeyDown += StartChargeHardAttack;
        _inputActions.AlternativeKeyUp += () => AttemptAttack(CalculateHardAttackDamage());
        _inputActions.AttackKeyDown += () => AttemptAttack(_meleeAttackParameters.LightAttackDamage);
    }

    public override void _ExitTree()
    {
        _attackCooldownTimer.Timeout -= CooldownAttackTimeout;
        _inputActions.AlternativeKeyDown -= StartChargeHardAttack;
        _inputActions.AlternativeKeyUp -= () => AttemptAttack(CalculateHardAttackDamage());
        _inputActions.AttackKeyDown -= () => AttemptAttack(_meleeAttackParameters.LightAttackDamage);
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
        int damage = (int)Mathf.Lerp(_meleeAttackParameters.HardAttackMinDamage, _meleeAttackParameters.HardAttackMaxDamage, chargePercentage);

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
        {
            _audioPlayer.PlayAudio(_meleeAttackParameters.MissAttackAudio);
            _audioPlayer.PlayAudio3DExceptClient(_meleeAttackParameters.MissAttackAudio);
            return;
        }

        PlayerHealth playerHealth = GetPlayerHealth(collider);
        playerHealth.DecreaseHealth(damage);

        _audioPlayer.PlayAudio(_meleeAttackParameters.HitAttackAudio);
        _audioPlayer.PlayAudio3DExceptClient(_meleeAttackParameters.HitAttackAudio);
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
