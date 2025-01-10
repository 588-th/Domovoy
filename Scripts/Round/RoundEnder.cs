using Godot;
using Godot.Collections;

public partial class RoundEnder : Node
{
    public override void _Ready()
    {
        GameEvent.Instance.playerDied += CheckPlayers;
    }

    public override void _ExitTree()
    {
        GameEvent.Instance.playerDied -= CheckPlayers;
    }

    private void CheckPlayers(long id)
    {
        Array<Node> players = GetTree().GetNodesInGroup("Player");

        int humanCount = 0;
        foreach (Node player in players)
            if (player.IsInGroup("Player:Human"))
                humanCount++;

        int monsterCount = 0;
        foreach (var player in players)
            if (player.IsInGroup("Player:Monster"))
                monsterCount++;

        if (humanCount == 0 || monsterCount == 0)
            EndRound();
    }

    private void EndRound()
    {
        GameState.Instance.StartLobby();
    }
}
