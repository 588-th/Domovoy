using Godot;

public partial class LookDirection : Node3D
{
    [Export] private Camera3D _camera;

    public override void _Process(double delta)
    {
        if (IsMultiplayerAuthority())
        {
            Position = _camera.Position;
            Rotation = _camera.Rotation;
        }
    }
}
