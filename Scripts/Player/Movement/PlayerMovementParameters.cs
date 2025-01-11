using Godot;

public partial class PlayerMovementParameters : Resource
{
    [Export] public float StandForwardSpeed = 5f;
    [Export] public float StandBackwardSpeed = 5f;
    [Export] public float CrouchSpeed = 2f;
    [Export] public float JumpSpeed = 5f;
    [Export] public float JumpForce = 5f;
    [Export] public int MaxJumps = 1;
    [Export] public float OnGroundAcceleration = 30f;
    [Export] public float OnAirAcceleration = 20f;
    [Export] public float Gravity = 9.8f;
    [Export] public float CurrentJumpForce = 5f;
    [Export] public float CurrentSpeed = 5f;
    [Export] public float CurrentAcceleration;

    [Export] public bool PlayAudioOnlyLocaly;
    [Export] public AudioStreamMP3 JumpAudio;
    [Export] public AudioStreamMP3 LandingAudio;
}
