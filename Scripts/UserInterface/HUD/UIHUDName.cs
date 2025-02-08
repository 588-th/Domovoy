using Godot;

public partial class UIHUDName : Node
{
    [Export] private HUDParameters _hudParameters;
    [Export] private Label _labelName;

    public override void _Ready()
    {
        _hudParameters.PlayerNameUpdated += UpdateNameData;
    }

    public override void _ExitTree()
    {
        _hudParameters.PlayerNameUpdated -= UpdateNameData;
    }

    private void UpdateNameData()
    {
        _labelName.Text = _hudParameters.PlayerName;
    }
}
