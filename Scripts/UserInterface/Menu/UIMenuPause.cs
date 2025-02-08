using Godot;

public partial class UIMenuPause : Control
{
    [Export] private InputActions _inputActions;

    [Export] private Control _UIWindowSettings;
    [Export] private Button _buttonStartRound;
    [Export] private Button _buttonSettings;
    [Export] private Button _buttonDisconnect;

    private bool _isPaused;
    private bool _isServer;

    public override void _Ready()
    {
        _buttonStartRound.Visible = false;
        if (Multiplayer.IsServer())
        {
            _isServer = true;
            _buttonStartRound.Visible = true;
            _buttonStartRound.Pressed += OnStartRoundButtonPressed;
        }

        _inputActions.EscapeKeyDown += OnEscapeKeyPressed;
        _buttonSettings.Pressed += OnSettingsButtonPressed;
        _buttonDisconnect.Pressed += OnDisconnectButtonPressed;

        HidePauseMenu();
    }

    public override void _ExitTree()
    {

        if (_isServer)
            _buttonStartRound.Pressed -= OnStartRoundButtonPressed;

        _inputActions.EscapeKeyDown -= OnEscapeKeyPressed;
        _buttonSettings.Pressed -= OnSettingsButtonPressed;
        _buttonDisconnect.Pressed -= OnDisconnectButtonPressed;
    }

    private void OnEscapeKeyPressed()
    {
        if (_isPaused)
            HidePauseMenu();
        else
            ShowPauseMenu();
    }

    public void ShowPauseMenu()
    {
        _isPaused = true;
        Input.MouseMode = Input.MouseModeEnum.Visible;
        Show();
    }

    public void HidePauseMenu()
    {
        _isPaused = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;
        Hide();
    }

    private void OnStartRoundButtonPressed()
    {
        if (Multiplayer.IsServer())
            GameStage.Instance.ChangeStateToRound();
    }

    private void OnSettingsButtonPressed()
    {
        _UIWindowSettings.Show();
    }

    private void OnDisconnectButtonPressed()
    {
        if (Multiplayer.IsServer())
            MultiplayerConnection.Instance.CloseServer();
        else
            MultiplayerConnection.Instance.CloseClient();
    }
}
