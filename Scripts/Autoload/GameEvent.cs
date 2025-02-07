using Godot;
using System;

public partial class GameEvent : Node
{
    public static GameEvent Instance { get; private set; }

	public Action<long> PlayerDied;
	public Action ShieldFixed;

    public override void _Ready()
    {
        Instance = this;
    }
}
