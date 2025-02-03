using Godot;

public partial class SettingsAudio : Node
{
    public static SettingsAudio Instance { get; private set; }
    public int Master = 0;
    public int Interface = 0;
    public int Effects = 0;
    public int VoiceChat = 0;

    public override void _Ready()
    {
        Instance = this;
    }
}
