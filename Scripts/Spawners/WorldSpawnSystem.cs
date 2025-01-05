using Godot;

public partial class WorldSpawnSystem : Node
{
    [Export] private LevelSpawner _levelSpawner;
    [Export] private PackedScene _lobbyLevelScene;
    [Export] private PackedScene _roundLevelScene;

    public override void _Ready()
    {
        MultiplayerConnection.Instance.ClientClosed += RefreshWorldScene;
        MultiplayerConnection.Instance.ServerClosed += RefreshWorldScene;

        GameState.Instance.LobbyStarted += CreateLobby;
        GameState.Instance.RoundStarted += StartRound;
    }

    public override void _ExitTree()
    {
        MultiplayerConnection.Instance.ClientClosed -= RefreshWorldScene;
        MultiplayerConnection.Instance.ServerClosed -= RefreshWorldScene;

        GameState.Instance.LobbyStarted -= CreateLobby;
        GameState.Instance.RoundStarted -= StartRound;
    }

    private void RefreshWorldScene()
    {
        GetTree().ReloadCurrentScene();
    }

    private void CreateLobby()
    {
        _levelSpawner.DespawnLevel();
        _levelSpawner.SpawnLevel(_lobbyLevelScene);
    }

    public void StartRound()
    {
        _levelSpawner.DespawnLevel();
        _levelSpawner.SpawnLevel(_roundLevelScene);
    }
}
