using Godot;

public partial class UICreateServer : Control
{
    [Export] private Node _mainManu;
    [Export] private Control _UIMenu;

    [Export] private Button _createServerButton;
    [Export] private Button _cancelButton;

    [Export] private LineEdit _ipLabel;
    [Export] private LineEdit _portLabel;

    public override void _Ready()
    {
        _createServerButton.Pressed += OnCreateServerButtonPressed;
        _cancelButton.Pressed += OnCancelButtonPressed;
    }

    public override void _ExitTree()
    {
        _createServerButton.Pressed -= OnCreateServerButtonPressed;
        _cancelButton.Pressed -= OnCancelButtonPressed;
    }

    private void OnCreateServerButtonPressed()
    {
        MultiplayerConnection.Instance.CreateServer(int.Parse(_portLabel.Text));
        GameState.Instance.StartLobby();
        _mainManu.QueueFree();
    }

    private void OnCancelButtonPressed()
    {
        Hide();
        _UIMenu.Show();
    }
}
