using Godot;
using System;

public partial class GameEvent : Node
{
    public static GameEvent Instance { get; private set; }

	public Action<long> playerDied;

    public override void _Ready()
    {
        Instance = this;
    }

    public void InvokePlayerDied(long playerID)
	{
        playerDied?.Invoke(playerID);
    }
}
