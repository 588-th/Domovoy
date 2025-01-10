using Godot;

public partial class BulletCollided : Node
{
    [Export] private RigidBody3D _bulletRigidBody;
    [Export] private BulletDestroy _bulletDestroy;
    [Export] private AudioStreamMP3 _hitAudio;

    public override void _Ready()
    {
        _bulletRigidBody.BodyEntered += OnBodyEntered;
    }

    public override void _ExitTree()
    {
        _bulletRigidBody.BodyEntered -= OnBodyEntered;
    }

    private void OnBodyEntered(Node body)
    {
        GD.Print("Hit");
        if (IsPlayerHitbox(body))
        {
            GD.Print("HitPlayer");
            int plaeyrHealth = DecreasePlayerHealth(body);
            if (plaeyrHealth > 0)
                PlayHitAudio(body);
        }

        _bulletDestroy.Destroy();
    }

    private bool IsPlayerHitbox(Node node)
    {
        return node.IsInGroup("Hitbox:Player");
    }

    private int DecreasePlayerHealth(Node body)
    {
        var playerRootNode = body.GetMeta("PlayerRoot");
        Node playerRoot = body.GetNode(playerRootNode.ToString());

        var playerHealthNode = playerRoot.GetMeta("PlayerHealth");
        PlayerHealth playerHealth = playerRoot.GetNode(playerHealthNode.ToString()) as PlayerHealth;

        playerHealth.DecreaseHealth(10);
        return playerHealth.CurrentHealth;
    }

    private void PlayHitAudio(Node body)
    {
        var playerRootNode = body.GetMeta("PlayerRoot");
        Node playerRoot = body.GetNode(playerRootNode.ToString());

        var audioPlayerNode = playerRoot.GetMeta("AudioPlayer");
        AudioPlayer audioPlayer = playerRoot.GetNode(audioPlayerNode.ToString()) as AudioPlayer;

        audioPlayer.PlayAudio(_hitAudio);
        audioPlayer.PlayAudio3DExceptClient(_hitAudio);
    }
}
