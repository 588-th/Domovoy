using Godot;

public partial class PlayerMeleeAttack : Node
{
    [Export] private InputActions _inputActions;
    [Export] private MeleeAttackActions _meleeAttackActions;
    [Export] private PlayerPropInteractor _playerPropInteractor;
    [Export] private Timer _attackCooldownTimer;
    [Export] private RayCast3D _rayOfAttack;

    [ExportGroup("Parameters")]
    [Export] private float _attackCooldownTime;
    [Export] private int _attackDamage;

    [ExportGroup("Audio")]
    [Export] private AudioPlayer3D _audioPlayer3D;
    [Export] private AudioStreamMP3 _missAttackAudio;
    [Export] private AudioStreamMP3 _hitAttackAudio;

    private bool _isCooldownAttackTimeout = true;

    public override void _Ready()
    {
        _attackCooldownTimer.WaitTime = _attackCooldownTime;

        _attackCooldownTimer.Timeout += CooldownAttackTimeout;
        _inputActions.AttackKeyDown += () => AttemptAttack(_attackDamage);
    }

    public override void _ExitTree()
    {
        _attackCooldownTimer.Timeout -= CooldownAttackTimeout;
        _inputActions.AttackKeyDown -= () => AttemptAttack(_attackDamage);
    }

    private void CooldownAttackTimeout()
    {
        _isCooldownAttackTimeout = true;
        _attackCooldownTimer.Stop();
        _meleeAttackActions.InvokeAction("CooldownEnd");
    }

    private void AttemptAttack(int damage)
    {
        if (!_isCooldownAttackTimeout || _playerPropInteractor.DraggedObject != null)
            return;

        PerformAttack(damage);
    }

    private void PerformAttack(int damage)
    {
        Node collider = _rayOfAttack.GetCollider() as Node;

        _isCooldownAttackTimeout = false;
        _attackCooldownTimer.Start();

        _meleeAttackActions.InvokeAction("CooldownStart");
        _meleeAttackActions.InvokeAction("Attack");

        if (!IsLookingAtPlayer())
        {
            _audioPlayer3D.PlayAudio3D(_missAttackAudio);
            return;
        }

        PlayerHealth playerHealth = GetPlayerHealth(collider);
        playerHealth.DecreaseHealth(damage);

        _audioPlayer3D.PlayAudio3D(_hitAttackAudio);
    }

    private bool IsLookingAtPlayer()
    {
        if (_rayOfAttack.GetCollider() is not Node collider)
            return false;

        return collider.IsInGroup("Hitbox:Player");
    }

    private PlayerHealth GetPlayerHealth(Node collider)
    {
        Node playerRoot = collider.GetNode(collider.GetMeta("PlayerRoot").ToString());
        return playerRoot?.GetNode(playerRoot.GetMeta("PlayerHealth").ToString()) as PlayerHealth;
    }
}
