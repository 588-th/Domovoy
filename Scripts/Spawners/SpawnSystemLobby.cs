using Godot;
using Godot.Collections;

public partial class SpawnSystemLobby : Node
{
    [ExportGroup("Spawners")]
    [Export] private SpawnerPlayer _spawnerPlayerLobby;

    [ExportGroup("Scenes")]
    [Export] private PackedScene _playerSceneLobby;

    [ExportGroup("Markers")]
    [Export] private Array<Marker3D> _spawnMarkersLobby;

    private int _spawnCount;

    public override void _Ready()
    {
        Multiplayer.PeerConnected += OnPeerConnected;
        Multiplayer.PeerDisconnected += OnPeerDisconnected;

        SpawnPlayers();
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

        _spawnerPlayerLobby.SpawnPlayer(1, _playerSceneLobby, _spawnMarkersLobby[0]);
        _spawnCount++;

        int[] peers = Multiplayer.GetPeers();
        foreach (var peer in peers)
        {
            _spawnerPlayerLobby.SpawnPlayer(peer, _playerSceneLobby, _spawnMarkersLobby[_spawnCount]);
            _spawnCount++;
        }
    }

    private void OnPeerConnected(long peerID)
    {
        if (!Multiplayer.IsServer())
            return;

        _spawnerPlayerLobby.SpawnPlayer(peerID, _playerSceneLobby, _spawnMarkersLobby[_spawnCount]);
        _spawnCount++;
    }

    private void OnPeerDisconnected(long peerID)
    {
        if (!Multiplayer.IsServer())
            return;

        _spawnerPlayerLobby.DespawnPlayer((int)peerID);
        _spawnCount--;
    }
}