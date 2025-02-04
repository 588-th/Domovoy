using Godot;

public partial class UISettingsMultiplayer : Control
{
    [Export] private Control _settings;
    [Export] private LineEdit _ipLabel;
    [Export] private LineEdit _portLabel;
    [Export] private Button _closeButton;

    public override void _Ready()
    {
        _closeButton.Pressed += OnCloseButtonPressed;
    }

    public override void _ExitTree()
    {
        _closeButton.Pressed -= OnCloseButtonPressed;
    }

    private void InitializeUI()
    {
        _ipLabel.Text = SettingsMultiplayer.Instance.IpAddress;
        _portLabel.Text = SettingsMultiplayer.Instance.Port.ToString();
    }

    private void OnCloseButtonPressed()
    {
        _settings.Hide();
    }
}
