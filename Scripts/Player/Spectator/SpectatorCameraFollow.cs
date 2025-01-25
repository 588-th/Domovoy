using Godot;

public partial class SpectatorCameraFollow : Node
{
    [Export] private SpectatorPlayerChanger _spectatorPlayerChanger;
    [Export] private Camera3D _spectatorCamera;
    private Node3D _cameraHolder;
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
        _spectatorCamera.Rotation = _playerCamera.Rotation;
        _spectatorCamera.Position = _cameraHolder.Position;
    }

    private void StartCameraFollow(string playerRootPath)
    {
        Node playerRoot = GetNode(playerRootPath);

        var playerCameraMeta = playerRoot.GetMeta("PlayerCamera");
        _playerCamera = playerRoot.GetNode(playerCameraMeta.ToString()) as Camera3D;

        var cameraHolderMeta = playerRoot.GetMeta("CameraHolder");
        _cameraHolder = playerRoot.GetNode(cameraHolderMeta.ToString()) as Node3D;

        SetProcess(true);
    }

    private void StopCameraFollow()
    {
        _playerCamera = null;
        _cameraHolder = null;
        SetProcess(false);
    }
}
