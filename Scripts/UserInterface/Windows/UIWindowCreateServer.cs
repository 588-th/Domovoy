using Godot;

public partial class UIWindowCreateServer : Control
{
    [Export] private Button _buttonCreateServer;
    [Export] private Button _buttonClose;
    [Export] private LineEdit _lineEditIP;
    [Export] private LineEdit _lineEditPort;

    public override void _Ready()
    {
        _buttonCreateServer.Pressed += OnCreateServerButtonPressed;
        _buttonClose.Pressed += OnCloseButtonPressed;
    }

    public override void _ExitTree()
    {
        _buttonCreateServer.Pressed -= OnCreateServerButtonPressed;
        _buttonClose.Pressed -= OnCloseButtonPressed;
    }

    private void OnCreateServerButtonPressed()
    {
        MultiplayerConnection.Instance.CreateServer(int.Parse(_lineEditPort.Text));
        GameStage.Instance.ChangeStageToLobby();
    }

    private void OnCloseButtonPressed()
    {
        Hide();
    }
}
