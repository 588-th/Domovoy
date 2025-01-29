using Godot;

public partial class PlayerCamera : Node
{
    [Export] private Camera3D _playerCamera;
    [Export] private bool _playerCanMoveCamera = true;

    private float _sensitivityMultiplayer = 0.001f;

    public override void _Ready()
    {
        SettingsVideo.Instance.FOVUpdated += UpdateFOV;
        InitializeSettings();
    }

    public override void _ExitTree()
    {
        SettingsVideo.Instance.FOVUpdated -= UpdateFOV;
    }

    private void InitializeSettings()
    {
        _playerCamera.Current = true;
        Input.MouseMode = Input.MouseModeEnum.Captured;
        UpdateFOV();
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is not InputEventMouseMotion mouseMotion)
            return;

        if (_playerCanMoveCamera)
            RotateCamera(mouseMotion);
    }

    private void RotateCamera(InputEventMouseMotion mouseMotion)
    {
        float sensivity = SettingsMouse.Instance.Sensivity * _sensitivityMultiplayer;

        _playerCamera.RotateY(-mouseMotion.Relative.X * sensivity);

        float xRotation = Mathf.Clamp(
            _playerCamera.Rotation.X - mouseMotion.Relative.Y * sensivity,
            Mathf.DegToRad(-90), Mathf.DegToRad(90)
        );
        _playerCamera.Rotation = new Vector3(xRotation, _playerCamera.Rotation.Y, _playerCamera.Rotation.Z);
    }

    private void UpdateFOV()
    {
        _playerCamera.Fov = SettingsVideo.Instance.FOV;
    }
}
