using Godot;

public partial class BulletCollidedWithPlayer : Node
{
    [Export] private RigidBody3D _bulletRigidBody;
    [Export] private BulletDestroy _bulletDestroy;

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
            DecreasePlayerHealth(body);
        }
    }

    private bool IsPlayerHitbox(Node node)
    {
        return node.IsInGroup("Hitbox:Player");
    }

    private void DecreasePlayerHealth(Node body)
    {
        var playerRootNode = body.GetMeta("PlayerRoot");
        PlayerHealth playerHealth = body.GetNode(playerRootNode + "/ServerPart/Scripts/PlayerHealth") as PlayerHealth;
        playerHealth.DecreaseHealth(10);
    }
}
