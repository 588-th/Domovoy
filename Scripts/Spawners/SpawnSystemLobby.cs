using Godot;

public partial class SpawnSystemLobby : Node
{
    [Export] private SpawnerPlayer _playerSpawner;
    [Export] private PackedScene _playerLobbyScene;

    public override void _Ready()
    {
        SpawnPlayers();
        Multiplayer.PeerConnected += OnPeerConnected;
        Multiplayer.PeerDisconnected += OnPeerDisconnected;
    }

    public override void _ExitTree()
    {
        Multiplayer.PeerConnected -= OnPeerConnected;
        Multiplayer.PeerDisconnected -= OnPeerDisconnected;
    }

    private void SpawnPlayers()
    {
        if (!Multiplayer.IsServer())
            return;

        _playerSpawner.SpawnPlayer(1, _playerLobbyScene);
        foreach (int peer in Multiplayer.GetPeers())
            _playerSpawner.SpawnPlayer(peer, _playerLobbyScene);
    }

    private void OnPeerConnected(long peerID)
    {
        if (Multiplayer.IsServer())
            _playerSpawner.SpawnPlayer(peerID, _playerLobbyScene);
    }

    private void OnPeerDisconnected(long peerID)
    {
        if (Multiplayer.IsServer())
            _playerSpawner.DespawnPlayer((int)peerID);
    }
}