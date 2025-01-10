using Godot;

public partial class SpectatorCameraFollow : Node
{
    [Export] private SpectatorPlayerChanger _spectatorPlayerChanger;
    [Export] private Camera3D _spectatorCamera;
    private Camera3D _playerCamera;

    public override void _Ready()
    {
        _spectatorPlayerChanger.SpectatedPlayerChanged += StartCameraFollow;
        SetProcess(false);
    }

    public override void _ExitTree()
    {
        _spectatorPlayerChanger.SpectatedPlayerChanged -= StartCameraFollow;
        StopCameraFollow();
    }

    public override void _Process(double delta)
    {
        _spectatorCamera.Position = _playerCamera.Position;
        _spectatorCamera.Rotation = _playerCamera.Rotation;
    }

    private void StartCameraFollow(string playerRootPath)
    {
        Node playerRoot = GetNode(playerRootPath);

        var playerRootNode = playerRoot.GetMeta("PlayerCamera");
        _playerCamera = playerRoot.GetNode(playerRootNode.ToString()) as Camera3D;

        SetProcess(true);
    }

    private void StopCameraFollow()
    {
        _playerCamera = null;
        SetProcess(false);
    }
}
