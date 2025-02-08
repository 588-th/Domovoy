using Godot;

public partial class UIHUDAttack : Node
{
    [Export] private HUDParameters _hudParameters;
    [Export] private Label _labelAttack;

    public override void _Ready()
    {
        _hudParameters.PlayerAttackDataUpdated += UpdateAttackData;
    }

    public override void _ExitTree()
    {
        _hudParameters.PlayerAttackDataUpdated -= UpdateAttackData;
    }

    private void UpdateAttackData()
    {
        _labelAttack.Text = _hudParameters.PlayerAttack;
    }
}
