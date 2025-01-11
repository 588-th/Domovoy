using Godot;

public partial class UIAttackInfo : Node
{
    [Export] private HUDParameters _hudParameters;
    [Export] private Label _labelAttackInfo;

    public override void _Ready()
    {
        _hudParameters.PlayerAttackDataUpdated += UpdateAttackInfo;
    }

    public override void _ExitTree()
    {
        _hudParameters.PlayerAttackDataUpdated -= UpdateAttackInfo;
    }

    private void UpdateAttackInfo()
    {
        _labelAttackInfo.Text = _hudParameters.PlayerAttack;
    }
}
