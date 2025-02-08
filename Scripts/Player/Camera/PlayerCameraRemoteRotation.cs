using Godot;

public partial class PlayerCameraRemoteRotation : Node
{
    [Export] private Camera3D _playerCamera;
    [Export] private CharacterBody3D _playerBody;

    public override void _PhysicsProcess(double delta)
    {
        RemoteRotation();
    }

    private void RemoteRotation()
    {
        _playerBody.Rotation = new Vector3(_playerBody.Rotation.X, _playerCamera.Rotation.Y, _playerBody.Rotation.Z);
    }
}
