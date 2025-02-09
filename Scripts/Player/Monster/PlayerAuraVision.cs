using Godot;

public partial class PlayerAuraVision : Node
{
    [Export] private Camera3D _playerCamera;
    [Export] private InputActions _playerActions;

    [ExportGroup("Parameters")]
    [Export] private int _auraNumberMask = 20;
    [Export] public bool IsVisionEnable = true;

    private bool _isVisionOn;

    public override void _Ready()
    {
        _playerActions.ToggleMonsterVisionDown += ToggleMonsterVision;
    }

    public override void _ExitTree()
    {
        _playerActions.ToggleMonsterVisionDown -= ToggleMonsterVision;
    }

    public void ToggleMonsterVision()
    {
        if (_isVisionOn)
            ToggleOffMonsterVision();
        else if (IsVisionEnable)
            ToggleOnMonsterVision();
    }

    public void ToggleOnMonsterVision()
    {
        _isVisionOn = true;
        GlobalRpcFunctions.Instance.SetCullMaskCamera3D(_playerCamera.GetPath(), _auraNumberMask, _isVisionOn);
    }

    public void ToggleOffMonsterVision()
    {
        _isVisionOn = false;
        GlobalRpcFunctions.Instance.SetCullMaskCamera3D(_playerCamera.GetPath(), _auraNumberMask, _isVisionOn);
    }
}
