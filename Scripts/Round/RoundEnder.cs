using Godot;

public partial class RoundEnder : Node
{
    [Export] private Node _parentPlayerHumans;
    [Export] private Node _parentPlayerMonsters;

    public override void _Ready()
    {
        GameEvent.Instance.playerDied += (_) => CheckPlayers();
    }

    private void CheckPlayers()
    {
        if (_parentPlayerHumans.GetChildren().Count == 0
            || _parentPlayerMonsters.GetChildren().Count == 0)
            EndRound();
    }

    private void EndRound()
    {
        GameState.Instance.StartLobby();
    }
}
