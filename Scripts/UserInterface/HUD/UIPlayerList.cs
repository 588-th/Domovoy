using Godot;

public partial class UIPlayerList : Control
{
    [Export] private InputActions _inputActions;
    [Export] private ItemList _itemList;

    private bool _isPaused;

    public override void _Ready()
    {
        Hide();
        AddExistedPlayers();
        Multiplayer.PeerConnected += AddPlayer;
        Multiplayer.PeerDisconnected += RemovePlayer;
        _inputActions.EscapeKeyDown += OnEscapeKeyPressed;
    }

    public override void _ExitTree()
    {
        Multiplayer.PeerConnected -= AddPlayer;
        Multiplayer.PeerDisconnected -= RemovePlayer;
        _inputActions.EscapeKeyDown -= OnEscapeKeyPressed;
    }

    private void AddExistedPlayers()
    {
        AddPlayer(Multiplayer.GetUniqueId());
        foreach (long peerID in Multiplayer.GetPeers())
            AddPlayer(peerID);
    }

    private void AddPlayer(long peerID)
    {
        _itemList.AddItem(peerID.ToString());
    }

    private void RemovePlayer(long peerID)
    {
        for (int i = 0; i < _itemList.GetItemCount(); i++)
        {
            if (_itemList.GetItemText(i) == peerID.ToString())
            {
                _itemList.RemoveItem(i);
                break;
            }
        }
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
}
