using Godot;
using System;

public partial class SettingsVideo : Node
{
    public static SettingsVideo Instance { get; private set; }
    public Action FOVUpdated;

    private float _fov = 75;
    public float FOV
    { 
        get { return _fov; } 
        set { _fov = value; FOVUpdated?.Invoke(); } 
    }
    public string Resolution = "1152x648";

    public override void _Ready()
    {
        Instance = this;
    }
}
