using Godot;

public partial class PlayerBulletHit : Node
{
    [Export] private Area3D _humanArea;
    [Export] private CollisionShape3D coll;
    [Export] private PlayerHealth _playerHealth;
    [Export] private AudioPlayer _playerAudio;
    [Export] private AudioStreamMP3 _bulletHitAudio;
}
