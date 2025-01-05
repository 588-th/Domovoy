using Godot;

public partial class SettingsMultiplayer : Node
{
    public static SettingsMultiplayer Instance { get; private set; }

    public string IpAddress = "127.0.0.1";
    public int Port = 7000;

    public override void _Ready()
    {
        Instance = this;
    }
}
