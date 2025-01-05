using Godot;

public partial class SettingsMouse : Node
{
    public static SettingsMouse Instance { get; private set; }
    public float Sensivity = 3;

    public override void _Ready()
    {
        Instance = this;
    }
}
