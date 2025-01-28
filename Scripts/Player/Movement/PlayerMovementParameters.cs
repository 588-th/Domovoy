using Godot;

public partial class PlayerMovementParameters : Resource
{
    [Export] public float WalkSpeed = 3.5f;
    [Export] public float SneakSpeed = 2f;
    [Export] public float CrouchSpeed = 1f;
    [Export] public float JumpSpeed = 5f;
    [Export] public float JumpForce = 5f;
    [Export] public int MaxJumps = 1;
    [Export] public float OnGroundAcceleration = 30f;
    [Export] public float OnAirAcceleration = 20f;
    [Export] public float Gravity = 9.8f;
    [Export] public float CurrentJumpForce = 5f;
    [Export] public float CurrentSpeed = 5f;
    [Export] public float CurrentAcceleration;
}
