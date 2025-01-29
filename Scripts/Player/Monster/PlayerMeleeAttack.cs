using Godot;

public partial class PlayerMeleeAttack : Node
{
    [Export] private InputActions _inputActions;
    [Export] private PlayerPropInteractor _playerObjectInteractor;
    [Export] private RayCast3D _rayOfAttack;
    [Export] private Timer _attackCooldownTimer;
    [Export] private AudioPlayer3D _audioPlayer3D;
    [Export] private PlayerMeleeAttackParameters _meleeAttackParameters;

    private bool _isCooldownAttackTimeout = true;

    public override void _Ready()
    {
        _attackCooldownTimer.WaitTime = _meleeAttackParameters.AttackCooldownTime;

        _attackCooldownTimer.Timeout += CooldownAttackTimeout;
        _inputActions.AttackKeyDown += () => AttemptAttack(_meleeAttackParameters.LightAttackDamage);
    }

    public override void _ExitTree()
    {
        _attackCooldownTimer.Timeout -= CooldownAttackTimeout;
        _inputActions.AttackKeyDown -= () => AttemptAttack(_meleeAttackParameters.LightAttackDamage);
    }

    private void CooldownAttackTimeout()
    {
        _isCooldownAttackTimeout = true;
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
            _audioPlayer3D.PlayAudio3D(_meleeAttackParameters.MissAttackAudio);
            return;
        }

        PlayerHealth playerHealth = GetPlayerHealth(collider);
        playerHealth.DecreaseHealth(damage);

        _audioPlayer3D.PlayAudio3D(_meleeAttackParameters.HitAttackAudio);
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
