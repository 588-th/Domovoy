using Godot;

public partial class UINameInfo : Node
{
    [Export] private HUDParameters _hudParameters;
    [Export] private Label _labelNameInfo;

    public override void _Ready()
    {
        _hudParameters.PlayerNameUpdated += UpdatePlayerName;
    }

    public override void _ExitTree()
    {
        _hudParameters.PlayerNameUpdated -= UpdatePlayerName;
    }

    private void UpdatePlayerName()
    {
        _labelNameInfo.Text = _hudParameters.PlayerName;
    }
}
