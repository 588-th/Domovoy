using Godot;

public partial class GameStageSpawner : Node
{
    [Export] private LevelSpawner _levelSpawner;
    [Export] private PackedScene _lobbyLevelScene;
    [Export] private PackedScene _roundLevelScene;

    public override void _Ready()
    {
        GameStage.Instance.MainMenuStage += StartMainMenu;
        GameStage.Instance.LobbyStage += StartLobby;
        GameStage.Instance.RoundStage += StartRound;
    }

    public override void _ExitTree()
    {
        GameStage.Instance.MainMenuStage -= StartMainMenu;
        GameStage.Instance.LobbyStage -= StartLobby;
        GameStage.Instance.RoundStage -= StartRound;
    }

    private void StartMainMenu()
    {
        GetTree().ReloadCurrentScene();
    }

    private void StartLobby()
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
