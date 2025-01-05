using Godot;

public partial class UIMainMenu : Control
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
        Hide();
        _UICreateServer.Show();
    }

    private void OnConnectServerButtonPressed()
    {
        Hide();
        _UIConnectServer.Show();
    }

    private void OnSettingsButtonPressed()
    {
        Hide();
        _UISettings.Show();
    }

    private void OnExitButtonPressed()
    {
        GetTree().Quit();
    }
}
