using Godot;

public partial class UIMenu : Control
{
    [Export] private Control _settings;

    [Export] private Button _startRound;
    [Export] private Button _settingsButton;
    [Export] private Button _disconnectButton;

    public override void _Ready()
    {
        _startRound.Pressed += OnStartRoundButtonPressed;
        _settingsButton.Pressed += OnSettingsButtonPressed;
        _disconnectButton.Pressed += OnDisconnectButtonPressed;
    }

    public override void _ExitTree()
    {
        _startRound.Pressed -= OnStartRoundButtonPressed;
        _settingsButton.Pressed -= OnSettingsButtonPressed;
        _disconnectButton.Pressed -= OnDisconnectButtonPressed;
    }

    private void OnStartRoundButtonPressed()
    {
        if (Multiplayer.IsServer())
            GameState.Instance.StartRound();
    }

    private void OnSettingsButtonPressed()
    {
        Hide();
        _settings.Show();
    }

    private void OnDisconnectButtonPressed()
    {
        if (Multiplayer.IsServer())
            MultiplayerConnection.Instance.CloseServer();
        else
            MultiplayerConnection.Instance.CloseClient();
    }
}
