using Godot;

public partial class UIHealthInfo : Control
{
    [Export] private HUDParameters _hudParameters;
    [Export] private Label _labelHealthInfo;

    public override void _Ready()
    {
        _hudParameters.PlayerHealthUpdated += UpdateHealthInfo;
    }

    public override void _ExitTree()
    {
        _hudParameters.PlayerHealthUpdated -= UpdateHealthInfo;
    }

    private void UpdateHealthInfo()
    {
        _labelHealthInfo.Text = _hudParameters.PlayerHealth.ToString();
    }
}
