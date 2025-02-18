using Godot;
using System;

public partial class SettingsRound : Node
{
    public static SettingsRound Instance { get; private set; }

    public Action<PackedScene> MapUpdated;

    private PackedScene _map;

    public PackedScene Map
    {
        get { return _map; }
        set { _map = value; MapUpdated?.Invoke(_map); }
    }

    public override void _Ready()
    {
        Instance = this;
    }
}
