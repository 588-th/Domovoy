using Godot;

public partial class FirearmParameters : Node
{
    [Export] public int BulletsPerClip;
    [Export] public int TotalClips;
    [Export] public int CurrentBulletsInClip;
    [Export] public int CurrentClips;
    [Export] public float FireRate;
    [Export] public float BulletSpeed;
    [Export] public int LaserEnergy;
    [Export] public bool IsAutomatic;
    [Export] public bool IsCanSwitchAitomatic;

    [Export] public AudioStreamMP3 ShotAudio;
    [Export] public AudioStreamMP3 EmptyShotAudio;
    [Export] public AudioStreamMP3 ReloadAudio;
    [Export] public AudioStreamMP3 ToggleAutomaticAudio;
    [Export] public AudioStreamMP3 LaserOnAudio;
    [Export] public AudioStreamMP3 LaserOffAudio;
}
