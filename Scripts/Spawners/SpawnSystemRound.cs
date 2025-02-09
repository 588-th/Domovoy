using Godot;
using Godot.Collections;

public partial class SpawnSystemRound : Node
{
    [ExportGroup("Spawners")]
    [Export] private SpawnerItem _spawnerItem;
    [Export] private SpawnerPlayer _spawnerPlayerHuman;
    [Export] private SpawnerPlayer _spawnerPlayerMonster;
    [Export] private SpawnerSpectator _spawnerPlayerSpectator;

    [ExportGroup("Scenes")]
    [Export] private PackedScene _playerSceneHuman;
    [Export] private PackedScene _playerSceneMonster;
    [Export] private PackedScene _playerSceneSpectator;

    [ExportGroup("Markers")]
    [Export] private Array<Marker3D> _spawnMarkersHuman;
    [Export] private Array<Marker3D> _spawnMarkersMonster;

    public override void _Ready()
    {
        GameEvent.Instance.PlayerDied += OnPlayerDied;
        Multiplayer.PeerConnected += OnPeerConnected;
        Multiplayer.PeerDisconnected += OnPeerDisconnected;
    }

    public override void _ExitTree()
    {
        GameEvent.Instance.PlayerDied -= OnPlayerDied;
        Multiplayer.PeerConnected -= OnPeerConnected;
        Multiplayer.PeerDisconnected -= OnPeerDisconnected;
    }

    public void SpawnItems()
    {
        _spawnerItem.SpawnItems();
    }

    public void SpawnPlayers(Dictionary<int, string> playerRoles)
    {
        Array<Marker3D> shuffledHumanMarkers = _spawnMarkersHuman.Duplicate();
        Array<Marker3D> shuffledMonsterMarkers = _spawnMarkersMonster.Duplicate();

        shuffledHumanMarkers.Shuffle();
        shuffledMonsterMarkers.Shuffle();

        int humanIndex = 0;
        int monsterIndex = 0;

        foreach (var player in playerRoles)
        {
            int peerID = player.Key;
            string role = player.Value;

            if (role == "human" && humanIndex < shuffledHumanMarkers.Count)
            {
                SpawnHuman(peerID, shuffledHumanMarkers[humanIndex]);
                humanIndex++;
            }
            else if (role == "monster" && monsterIndex < shuffledMonsterMarkers.Count)
            {
                SpawnMonster(peerID, shuffledMonsterMarkers[monsterIndex]);
                monsterIndex++;
            }
        }
    }

    private void SpawnHuman(int peerID, Marker3D marker)
    {
        _spawnerPlayerHuman.SpawnPlayer(peerID, _playerSceneHuman, marker);
    }

    private void SpawnMonster(int peerID, Marker3D marker)
    {
        _spawnerPlayerMonster.SpawnPlayer(peerID, _playerSceneMonster, marker);
    }

    private void SpawnSpectator(long peerID)
    {
        _spawnerPlayerSpectator.SpawnSpectator(peerID, _playerSceneSpectator);
    }

    private async void OnPlayerDied(long peerID)
    {
        await ToSignal(GetTree(), "process_frame");
        SpawnSpectator(peerID);
    }

    private void OnPeerConnected(long peerID)
    {
        if (Multiplayer.IsServer())
            SpawnSpectator(peerID);
    }

    private void OnPeerDisconnected(long peerID)
    {
        if (Multiplayer.IsServer())
        {
            _spawnerPlayerHuman.DespawnPlayer((int)peerID);
            _spawnerPlayerMonster.DespawnPlayer((int)peerID);
            _spawnerPlayerSpectator.DespawnSpectator((int)peerID);
        }
    }
}
