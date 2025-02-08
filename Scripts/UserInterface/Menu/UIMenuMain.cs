using Godot;

public partial class UIMenuMain : Control
{
    [Export] private Button _createServerButton;
    [Export] private Button _createClientButton;
    [Export] private Button _settingsButton;
    [Export] private Button _exitButton;

    [Export] private Control _UICreateServer;
    [Export] private Control _UIConnectServer;
    [Export] private Control _UISettings;

    public override void _Ready()
    {
        _createServerButton.Pressed += OnCreateServerButtonPressed;
        _createClientButton.Pressed += OnConnectServerButtonPressed;
        _settingsButton.Pressed += OnSettingsButtonPressed;
        _exitButton.Pressed += OnExitButtonPressed;
    }

    public override void _ExitTree()
    {
        _createServerButton.Pressed -= OnCreateServerButtonPressed;
        _createClientButton.Pressed -= OnConnectServerButtonPressed;
        _settingsButton.Pressed -= OnSettingsButtonPressed;
        _exitButton.Pressed -= OnExitButtonPressed;
    }

    private void OnCreateServerButtonPressed()
    {
        _UIConnectServer.Hide();
        _UISettings.Hide();
        _UICreateServer.Show();
    }

    private void OnConnectServerButtonPressed()
    {
        _UICreateServer.Hide();
        _UISettings.Hide();
        _UIConnectServer.Show();
    }

    private void OnSettingsButtonPressed()
    {
        _UICreateServer.Hide();
        _UIConnectServer.Hide();
        _UISettings.Show();
    }

    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}
