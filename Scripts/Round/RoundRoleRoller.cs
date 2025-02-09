using Godot;
using Godot.Collections;

public partial class RoundRoleRoller : Node
{
    private int _monstersCount;
    private int _humanCount;

    public Dictionary<int, string> RolleForPeers()
    {
        Dictionary<int, string> roles = new();
        Array<string> availableRoles = new();

        int[] peers = GetPeers();
        DetermineRoleCount(peers.Length);

        availableRoles.Resize(_monstersCount + _humanCount);
        for (int i = 0; i < _monstersCount; i++)
            availableRoles[i] = "monster";
        for (int i = _monstersCount; i < availableRoles.Count; i++)
            availableRoles[i] = "human";

        availableRoles.Shuffle();

        for (int i = 0; i < peers.Length; i++)
            roles[peers[i]] = availableRoles[i];

        return roles;
    }

    private void DetermineRoleCount(int playerCount)
    {
        _monstersCount = Mathf.Max(1, playerCount / 3);
        _humanCount = playerCount - _monstersCount;
    }

    private int[] GetPeers()
    {
        int[] clients = Multiplayer.GetPeers();
        int[] peers = new int[clients.Length + 1];
        peers[0] = 1;
        clients.CopyTo(peers, 1);
        return peers;
    }
}
