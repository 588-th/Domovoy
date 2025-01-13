using Godot;

public partial class UIMenu : Control
{
    [Export] private Control _settings;

    [Export] private Button _startRound;
    [Export] private Button _settingsButton;
    [Export] private Button _disconnectButton;

    public override void _Ready()
    {
        if (Multiplayer.IsServer())
        {
            _startRound.Visible = true;
            _startRound.Pressed += OnStartRoundButtonPressed;
        }

        _settingsButton.Pressed += OnSettingsButtonPressed;
        _disconnectButton.Pressed += OnDisconnectButtonPressed;
    }

    public override void _ExitTree()
    {
        if (Multiplayer.IsServer())
            _startRound.Pressed -= OnStartRoundButtonPressed;
        _settingsButton.Pressed -= OnSettingsButtonPressed;
        _disconnectButton.Pressed -= OnDisconnectButtonPressed;
    }

    private void OnStartRoundButtonPressed()
    {
        if (Multiplayer.IsServer())
            GameStage.Instance.ChangeStateToRound();
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
