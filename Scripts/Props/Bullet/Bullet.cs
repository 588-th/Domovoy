using Godot;

public partial class Bullet : Node
{
    [Export] private RigidBody3D _bulletRigidBody;
    [Export] private Timer _bulletLife;

    public override void _Ready()
    {
        _bulletLife.Start();
        Subscribe();
    }

    public override void _ExitTree()
    {
        base._ExitTree();
        Unsubscribe();
    }

    private void Subscribe()
    {
        _bulletLife.Timeout += DestroyBullet;
        _bulletRigidBody.BodyEntered += OnBodyEntered;
    }

    private void Unsubscribe()
    {
        _bulletLife.Timeout -= DestroyBullet;
        _bulletRigidBody.BodyEntered -= OnBodyEntered;
    }

    private void DestroyBullet()
    {
        QueueFree();
    }

    private void OnBodyEntered(Node body)
    {
        _bulletLife.Stop();
        if (IsPlayer(body))
        {
            DecreasePlayerHealth(body);
            DestroyBullet();
        }
    }

    private bool IsPlayer(Node node)
    {
        return node.IsInGroup("Player");
    }

    private void DecreasePlayerHealth(Node body)
    {
        var playerRootNode = body.GetMeta("PlayerRoot");

        PlayerHealth playerHealth = body.GetNode(playerRootNode + "/ServerPart/Scripts/PlayerHealth") as PlayerHealth;
        playerHealth.DecreaseHealth(10);
        GD.Print("DecreaseHealth");
    }
}
