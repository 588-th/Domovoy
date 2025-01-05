using Godot;

public partial class UISettingsMultiplayer : Control
{
    [Export] private Control _menu;
    [Export] private Control _settings;

    [Export] private LineEdit _ipLabel;
    [Export] private LineEdit _portLabel;

    [Export] private Button _applyButton;
    [Export] private Button _backButton;

    public override void _Ready()
    {
        _applyButton.Pressed += OnApplyButtonPressed;
        _backButton.Pressed += OnBackButtonPressed;
    }

    public override void _ExitTree()
    {
        _applyButton.Pressed -= OnApplyButtonPressed;
        _backButton.Pressed -= OnBackButtonPressed;
    }

    private void InitializeUI()
    {
        _ipLabel.Text = SettingsMultiplayer.Instance.IpAddress;
        _portLabel.Text = SettingsMultiplayer.Instance.Port.ToString();
    }

    private void OnApplyButtonPressed()
    {
        SettingsMultiplayer.Instance.IpAddress = _ipLabel.Text;
        SettingsMultiplayer.Instance.Port = int.Parse(_portLabel.Text);
    }

    private void OnBackButtonPressed()
    {
        _menu.Show();
        _settings.Hide();
    }
}
