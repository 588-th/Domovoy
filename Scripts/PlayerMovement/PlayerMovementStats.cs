public class PlayerMovementStats
{
    public float StandBackwardSpeed = 10f;
    public float StandForwardSpeed = 8f;

    public float CrouchSpeed = 4f;
    public float CrouchScaleY = 4f;

    public float JumpSpeed = 10f;
    public float JumpForce = 6f;

    public float JumpCrouchForce = 8f;

    public float OnGroundAcceleration = 100f;
    public float OnAirAcceleration = 20f;
    public float OnWallAcceleration = 10f;

    public float Gravity = 9.8f;

    public float CurrentSpeed;
    public float CurrentAcceleration;
}
