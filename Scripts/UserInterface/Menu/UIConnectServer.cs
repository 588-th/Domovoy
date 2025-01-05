using Godot;

public partial class UIConnectServer : Control
{
    [Export] private Node _mainManu;
    [Export] private Control _UIMenu;

    [Export] private Button _connectServerButton;
    [Export] private Button _cancelButton;

    [Export] private LineEdit _ipLabel;
    [Export] private LineEdit _portLabel;

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
        MultiplayerConnection.Instance.CreateClient(_ipLabel.Text, int.Parse(_portLabel.Text));
        _mainManu.QueueFree();
    }

    private void OnCancelButtonPressed()
    {
        Hide();
        _UIMenu.Show();
    }
}
