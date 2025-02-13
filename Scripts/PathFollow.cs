using Godot;

public partial class PathFollow : Node
{
    [Export] private PathFollow3D _pathFollow3D;
    [Export] private double _speed;

    public override void _PhysicsProcess(double delta)
    {
        _pathFollow3D.Progress += (float)(_speed * delta);
    }
}
