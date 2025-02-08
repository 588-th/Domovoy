using Godot;

public partial class UIWindowCreateClient : Control
{
    [Export] private Node _levelRoot;
    [Export] private Button _buttonCreateClient;
    [Export] private Button _buttonClose;
    [Export] private LineEdit _lineEditIP;
    [Export] private LineEdit _lineEditPort;

    public override void _Ready()
    {
        _buttonCreateClient.Pressed += OnCreateClientButtonPressed;
        _buttonClose.Pressed += OnCloseButtonPressed;
    }

    public override void _ExitTree()
    {
        _buttonCreateClient.Pressed -= OnCreateClientButtonPressed;
        _buttonClose.Pressed -= OnCloseButtonPressed;
    }

    private void OnCreateClientButtonPressed()
    {
        MultiplayerConnection.Instance.CreateClient(_lineEditIP.Text, int.Parse(_lineEditPort.Text));
        _levelRoot.QueueFree();
    }

    private void OnCloseButtonPressed()
    {
        Hide();
    }
}
