using Godot;

public partial class UIConnectServer : Control
{
    [Export] private Node _mainMenu;
    [Export] private Button _connectServerButton;
    [Export] private Button _closeButton;
    [Export] private LineEdit _ipLineEdit;
    [Export] private LineEdit _portLineEdit;

    public override void _Ready()
    {
        _connectServerButton.Pressed += OnConnectServerButtonPressed;
        _closeButton.Pressed += OnCloseButtonPressed;
    }

    public override void _ExitTree()
    {
        _connectServerButton.Pressed -= OnConnectServerButtonPressed;
        _closeButton.Pressed -= OnCloseButtonPressed;
    }

    private void OnConnectServerButtonPressed()
    {
        MultiplayerConnection.Instance.ClientCreated += OnConnectionSucces;
        MultiplayerConnection.Instance.ClientCreateFailed += OnConnectionFailed;

        MultiplayerConnection.Instance.CreateClient(_ipLineEdit.Text, int.Parse(_portLineEdit.Text));
    }

    private void OnConnectionSucces()
    {
        _mainMenu.QueueFree();
        MultiplayerConnection.Instance.ClientCreated -= OnConnectionSucces;
        MultiplayerConnection.Instance.ClientCreateFailed -= OnConnectionFailed;
    }

    private void OnConnectionFailed()
    {
        MultiplayerConnection.Instance.ClientCreated -= OnConnectionSucces;
        MultiplayerConnection.Instance.ClientCreateFailed -= OnConnectionFailed;
    }

    private void OnCloseButtonPressed()
    {
        Hide();
    }
}
