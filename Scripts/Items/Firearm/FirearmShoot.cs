using Godot;

public partial class FirearmShoot : Node
{
    [Export] private Firearm _firearm;
    [Export] private FirearmParameters _firearmParameters;
    [Export] private AudioPlayer _audioPlayer;

    [Export] private Timer _shootCooldownTimer;
    [Export] private Marker3D _bulletSpawnPosition;
    [Export] private PackedScene _bulletScene;

    private bool _isCooldownShootTimeout = true;

    public override void _EnterTree()
    {
        _shootCooldownTimer.Timeout += CooldownShootTimeout;
    }

    public override void _ExitTree()
    {
        _shootCooldownTimer.Timeout -= CooldownShootTimeout;
    }

    public void OnAttackKeyDown()
    {
        if (_firearmParameters.IsAutomatic)
            _shootCooldownTimer.Timeout += Shoot;

        Shoot();
    }

    public void OnAttackKeyUp()
    {
        if (_firearmParameters.IsAutomatic)
            _shootCooldownTimer.Timeout -= Shoot;
    }

    private void Shoot()
    {
        if (!_isCooldownShootTimeout)
            return;

        if (_firearmParameters.CurrentBulletsInClip <= 0)
        {
            _audioPlayer.PlayAudio3DForAll(_firearmParameters.EmptyShotAudio, true);
            return;
        }

        _isCooldownShootTimeout = false;
        _shootCooldownTimer.Start();

        SpawnBullet();
        _firearmParameters.CurrentBulletsInClip--;
        _audioPlayer.PlayAudio3DForAll(_firearmParameters.ShotAudio, true);

        _firearm.ItemUsed?.Invoke();
    }

    private void SpawnBullet()
    {
        var bullet = _bulletScene.Instantiate<RigidBody3D>();

        Node holderProps = GetTree().GetFirstNodeInGroup("Holder:Props");
        holderProps.AddChild(bullet, true);

        bullet.GlobalTransform = _firearm.GlobalTransform;
        bullet.ApplyCentralImpulse(_firearm.GlobalTransform.Basis * new Vector3(0, 0, -_firearmParameters.BulletSpeed));
    }

    private void CooldownShootTimeout()
    {
        _isCooldownShootTimeout = true;
    }
}
