using Godot;

public partial class MonsterVision : Node
{
    [Export] private Camera3D _playerCamera;
    [Export] private InputActions _playerActions;
    [Export] private int _auraNumberMask;

    private bool _isVisionEnable;

    public override void _Ready()
    {
        _playerActions.ToggleMonsterVisionDown += ToggleMonsterVision;
    }

    public override void _ExitTree()
    {
        _playerActions.ToggleMonsterVisionDown -= ToggleMonsterVision;
    }

    private void ToggleMonsterVision()
    {
        if (_isVisionEnable)
            ToggleOffMonsterVision();
        else
            ToggleOnMonsterVision();
    }

    private void ToggleOnMonsterVision()
    {
        _isVisionEnable = true;
        RpcFunctions.Instance.SetCullMaskCamera3D(_playerCamera.GetPath(), _auraNumberMask, _isVisionEnable);
    }

    private void ToggleOffMonsterVision()
    {
        _isVisionEnable = false;
        RpcFunctions.Instance.SetCullMaskCamera3D(_playerCamera.GetPath(), _auraNumberMask, _isVisionEnable);
    }
}
