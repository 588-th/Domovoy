using Godot;
using Godot.Collections;

public partial class RoundController : Node
{
    [Export] private RoundRoleRoller _roundRoleRoller;
    [Export] private SpawnSystemRound _spawnSystemRound;

    public override void _Ready()
    {
        if (Multiplayer.IsServer())
            StartRound();
    }

    private void StartRound()
    {
        Dictionary<int, string> playerRoles = _roundRoleRoller.RolleForPeers();
        _spawnSystemRound.SpawnItems();
        _spawnSystemRound.SpawnPlayers(playerRoles);
    }
}
