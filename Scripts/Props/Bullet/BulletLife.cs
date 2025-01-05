using Godot;

public partial class BulletLife : Node
{
    [Export] private Timer _bulletLife;
    [Export] private BulletDestroy _bulletDestroy;

    public override void _Ready()
    {
        _bulletLife.Start();
        Subscribe();
    }

    public override void _ExitTree()
    {
        Unsubscribe();
    }

    private void Subscribe()
    {
        _bulletLife.Timeout += _bulletDestroy.Destroy;
    }

    private void Unsubscribe()
    {
        _bulletLife.Timeout -= _bulletDestroy.Destroy;
    }
}
