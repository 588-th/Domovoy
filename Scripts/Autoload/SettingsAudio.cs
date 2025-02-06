using Godot;

public partial class SettingsAudio : Node
{
    public static SettingsAudio Instance { get; private set; }

    public int DefaultMaster { get; private set; } = 0;
    public int DefaultInterface { get; private set; } = 0;
    public int DefaultEffects { get; private set; } = 0;
    public int DefaultVoiceChat { get; private set; } = 0;
    public bool DefaultMuteYourself{ get; private set; } = true;

    public int Master = 0;
    public int Interface = 0;
    public int Effects = 0;
    public int VoiceChat = 0;
    public bool MuteYourself = true;

    public override void _Ready()
    {
        Instance = this;
    }
}
