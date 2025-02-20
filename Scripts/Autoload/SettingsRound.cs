using Godot;
using System;

public partial class SettingsRound : Node
{
    public static SettingsRound Instance { get; private set; }

    public Action<PackedScene> MapUpdated;

    public int HeadFlashlightBattery;
    public int DefaultHeadFlashlightBattery = 60;

    public int HumanHealth;
    public int DefaultHumanHealth = 100;

    public int MonsterHealth;
    public int DefaultMonsterHealth = 100;

    public int MonsterDamage;
    public int DefaultMonsterDamage = 35;

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
