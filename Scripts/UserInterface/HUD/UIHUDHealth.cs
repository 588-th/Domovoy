using Godot;

public partial class UIHUDHealth : Control
{
    [Export] private HUDParameters _hudParameters;
    [Export] private Label _labelHealth;

    public override void _Ready()
    {
        _hudParameters.PlayerHealthUpdated += UpdateHealthData;
    }

    public override void _ExitTree()
    {
        _hudParameters.PlayerHealthUpdated -= UpdateHealthData;
    }

    private void UpdateHealthData()
    {
        _labelHealth.Text = _hudParameters.PlayerHealth.ToString();
    }
}
