using Godot;

public partial class MonsterVision : Node
{
    [Export] private Camera3D _playerCamera;
    [Export] private InputActions _playerActions;
    [Export] private int _auraNumberMask;

    private bool _isVisionEnable;

    public override void _Ready()
    {
        _playerActions.ToggleMonsterVisionDown += ToggleVisionMode;
    }

    public override void _ExitTree()
    {
        _playerActions.ToggleMonsterVisionDown -= ToggleVisionMode;
    }

    private void ToggleVisionMode()
    {
        _isVisionEnable = !_isVisionEnable;
        _playerCamera.SetCullMaskValue(_auraNumberMask, _isVisionEnable);
    }
}
