using Godot;

public partial class BulletCollided : Node
{
    [Export] private RigidBody3D _bulletRigidBody;
    [Export] private BulletDestroy _bulletDestroy;

    [ExportGroup("Audio")]
    [Export] private PackedScene _audio3DSourseScene;
    [Export] private AudioStreamMP3 _hitHumanAudio;
    [Export] private AudioStreamMP3 _hitMonsterAudio;
    [Export] private AudioStreamMP3 _hitObjectAudio;

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
        if (IsPlayerHitbox(body))
        {
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
        return playerHealth.GetCurrentHealth();
    }

    private void PlayHitAudio(Node body)
    {
        Node3D audio3DSourse = _audio3DSourseScene.Instantiate() as Node3D;
        Node holderProps = GetTree().GetFirstNodeInGroup("Holder:Props");
        holderProps.AddChild(audio3DSourse, true);
        audio3DSourse.GlobalPosition = _bulletRigidBody.GlobalPosition;

        AudioPlayer3D audioPlayer3D = audio3DSourse.FindChild("Audio3D") as AudioPlayer3D;
        if (body.IsInGroup("Hitbox:PlayerHuman"))
            audioPlayer3D.PlayAudio3D(_hitHumanAudio);
        else if (body.IsInGroup("Hitbox:PlayerMonster"))
            audioPlayer3D.PlayAudio3D(_hitMonsterAudio);
    }
}
