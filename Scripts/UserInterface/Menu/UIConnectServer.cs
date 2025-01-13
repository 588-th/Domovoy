using Godot;

public partial class UIConnectServer : Control
{
    [Export] private Node _mainMenu;
    [Export] private Control _uiMenu;

    [Export] private Button _connectServerButton;
    [Export] private Button _cancelButton;

    [Export] private LineEdit _ipLineEdit;
    [Export] private LineEdit _portLineEdit;

    public override void _Ready()
    {
        _connectServerButton.Pressed += OnConnectServerButtonPressed;
        _cancelButton.Pressed += OnCancelButtonPressed;
    }

    public override void _ExitTree()
    {
        _connectServerButton.Pressed -= OnConnectServerButtonPressed;
        _cancelButton.Pressed -= OnCancelButtonPressed;
    }

    private void OnConnectServerButtonPressed()
    {
        MultiplayerConnection.Instance.ClientCreated += OnConnectionSucces;
        MultiplayerConnection.Instance.ClientCreateFailed += OnConnectionFailed;

        MultiplayerConnection.Instance.CreateClient(_ipLineEdit.Text, int.Parse(_portLineEdit.Text));
    }

    private void OnConnectionSucces()
    {
        GD.Print("S");
        _mainMenu.QueueFree();
        MultiplayerConnection.Instance.ClientCreated -= OnConnectionSucces;
        MultiplayerConnection.Instance.ClientCreateFailed -= OnConnectionFailed;
    }

    private void OnConnectionFailed()
    {
        GD.Print("F");

        MultiplayerConnection.Instance.ClientCreated -= OnConnectionSucces;
        MultiplayerConnection.Instance.ClientCreateFailed -= OnConnectionFailed;
    }

    private void OnCancelButtonPressed()
    {
        Hide();
        _uiMenu.Show();
    }
}
