using Godot;

public partial class BulletDestroy : Node
{
    [Export] private RigidBody3D _bulletRigidBody;

    public void Destroy()
    {
        _bulletRigidBody.QueueFree();
    }
}
