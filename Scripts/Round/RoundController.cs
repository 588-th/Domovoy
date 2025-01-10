using Godot;
using System;
using System.Collections.Generic;

public partial class RoundController : Node
{
    [Export] private RoundRoleRoller _roundRoleRoller;
    [Export] private RoundSpawnSystem _roundSpawnSystem;
    [Export] private ItemSpawner _itemSpawner;

    public override void _Ready()
    {
        if (Multiplayer.IsServer())
            StartRound();
    }

    private void StartRound()
    {
        Dictionary<int, string> playerRoles = GetRoles();
        _itemSpawner.SpawnItems();
        SpawnPlayers(playerRoles);
    }

    private Dictionary<int, string> GetRoles()
    {
        int[] clients = Multiplayer.GetPeers();
        int[] peers = new int[clients.Length + 1];
        peers[0] = 1;
        Array.Copy(clients, 0, peers, 1, clients.Length);
        Dictionary<int, string> playerRoles = _roundRoleRoller.RolleRoles(peers);

        return playerRoles;
    }

    private void SpawnPlayers(Dictionary<int, string> playerRoles)
    {
        foreach (var playerRole in playerRoles)
        {
            if (playerRole.Value == "monster")
                _roundSpawnSystem.SpawnMonster(playerRole.Key);
            else
                _roundSpawnSystem.SpawnHuman(playerRole.Key);
        }
    }
}
