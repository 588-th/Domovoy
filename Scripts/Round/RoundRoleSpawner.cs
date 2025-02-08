using Godot;

public partial class RoundRoleSpawner : Node
{
    [Export] private SpawnerPlayer _playerHumanSpawner;
    [Export] private SpawnerPlayer _playerMonsterSpawner;
    [Export] private SpawnerSpectator _playerSpectatorSpawner;
    [Export] private PackedScene _playerHumanScene;
    [Export] private PackedScene _playerMonsterScene;
    [Export] private PackedScene _playerSpectatorScene;

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

    public void SpawnHuman(int peersId)
    {
        _playerHumanSpawner.SpawnPlayer(peersId, _playerHumanScene);
    }

    public void SpawnMonster(int peersId)
    {
        _playerMonsterSpawner.SpawnPlayer(peersId, _playerMonsterScene);
    }

    private void SpawnSpectator(long peerID)
    {
        _playerSpectatorSpawner.SpawnSpectator(peerID, _playerSpectatorScene);
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
            _playerHumanSpawner.DespawnPlayer((int)peerID);
            _playerMonsterSpawner.DespawnPlayer((int)peerID);
            _playerSpectatorSpawner.DespawnSpectator((int)peerID);
        }
    }
}
