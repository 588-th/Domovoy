using Godot;
using System;
using System.Collections.Generic;

public partial class RoundRoleRoller : Node
{
    private int _monstersCount = 1;

    public Dictionary<int, string> RolleRoles(int[] peers)
    {
        Random random = new Random();
        Dictionary<int, string> roles = new Dictionary<int, string>();

        List<string> availableRoles = new List<string>();

        for (int i = 0; i < _monstersCount; i++)
            availableRoles.Add("monster");

        for (int i = 0; i < peers.Length - _monstersCount; i++)
            availableRoles.Add("human");

        for (int i = availableRoles.Count - 1; i > 0; i--)
        {
            int j = random.Next(i + 1);
            (availableRoles[i], availableRoles[j]) = (availableRoles[j], availableRoles[i]);
        }

        for (int i = 0; i < peers.Length; i++)
        {
            roles[peers[i]] = availableRoles[i];
        }

        return roles;
    }
}