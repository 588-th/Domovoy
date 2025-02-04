using Godot;

public partial class UICreateServer : Control
{
    [Export] private Node _mainMenu;
    [Export] private Button _createServerButton;
    [Export] private Button _closeButton;
    [Export] private LineEdit _ipLineEdit;
    [Export] private LineEdit _portLineEdit;

    public override void _Ready()
    {
        _createServerButton.Pressed += OnCreateServerButtonPressed;
        _closeButton.Pressed += OnCloseButtonPressed;
    }

    public override void _ExitTree()
    {
        _createServerButton.Pressed -= OnCreateServerButtonPressed;
        _closeButton.Pressed -= OnCloseButtonPressed;
    }

    private void OnCreateServerButtonPressed()
    {
        MultiplayerConnection.Instance.CreateServer(int.Parse(_portLineEdit.Text));
        GameStage.Instance.ChangeStageToLobby();
        _mainMenu.QueueFree();
    }

    private void OnCloseButtonPressed()
    {
        Hide();
    }
}
