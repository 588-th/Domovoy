using Godot;

public partial class BulletCollided : Node
{
    [Export] private RigidBody3D _bulletRigidBody;
    [Export] private Timer _bulletLife;

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
        _bulletLife.Stop();
    }
}
