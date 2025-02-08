using Godot;

public partial class UIWindowPlayerList : Control
{
    [Export] private InputActions _inputActions;
    [Export] private ItemList _itemListConnectedPeers;

    private bool _isPaused;

    public override void _Ready()
    {
        Multiplayer.PeerConnected += AddPlayer;
        Multiplayer.PeerDisconnected += RemovePlayer;
        _inputActions.EscapeKeyDown += OnEscapeKeyPressed;

        Hide();
        AddExistedPlayers();
    }

    public override void _ExitTree()
    {
        Multiplayer.PeerConnected -= AddPlayer;
        Multiplayer.PeerDisconnected -= RemovePlayer;
        _inputActions.EscapeKeyDown -= OnEscapeKeyPressed;
    }

    private void OnEscapeKeyPressed()
    {
        if (_isPaused)
            HidePlayerList();
        else
            ShowPlayerList();
    }

    public void ShowPlayerList()
    {
        _isPaused = true;
        Show();
    }

    public void HidePlayerList()
    {
        _isPaused = false;
        Hide();
    }

    private void AddExistedPlayers()
    {
        AddPlayer(Multiplayer.GetUniqueId());
        foreach (long peerID in Multiplayer.GetPeers())
            AddPlayer(peerID);
    }

    private void AddPlayer(long peerID)
    {
        _itemListConnectedPeers.AddItem(peerID.ToString());
    }

    private void RemovePlayer(long peerID)
    {
        for (int i = 0; i < _itemListConnectedPeers.GetItemCount(); i++)
        {
            if (_itemListConnectedPeers.GetItemText(i) == peerID.ToString())
            {
                _itemListConnectedPeers.RemoveItem(i);
                break;
            }
        }
    }
}
