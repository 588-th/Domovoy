using Godot;

public partial class PlayerMovement : Node
{
    [Export] public MovementActions MovementActions;
    [Export] public InputActions InputActions;
    [Export] public InputVector InputVector;
    [Export] public CharacterBody3D PlayerBody;

    [Export] public PlayerMovementParameters PlayerMovementParameters { get; private set; }
    public MovementState CurrentState { get; private set; }
    public IdleState IdleState { get; private set; }
    public WalkState WalkState { get; private set; }
    public JumpState JumpState { get; private set; }
    public SneakState SneakState { get; private set; }

    public bool IsGrounded { get; private set; }

    public override void _Ready()
    {
        InitializationFields();
    }

    public override void _PhysicsProcess(double delta)
    {
        CheckGround();
        CurrentState.Update(delta);
        PlayerBody.MoveAndSlide();
    }

    public override void _ExitTree()
    {
        CurrentState.Exit();
    }

    public void ChangeState(MovementState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }

    private async void InitializationFields()
    {
        IdleState = new IdleState(this);
        WalkState = new WalkState(this);
        JumpState = new JumpState(this);
        SneakState = new SneakState(this);

        CurrentState = JumpState;

        await ToSignal(GetTree(), "process_frame");
        CurrentState.Enter();
    }

    private void CheckGround()
    {
        bool lastFrame = IsGrounded;
        IsGrounded = PlayerBody.IsOnFloor();

        if (IsGrounded && lastFrame != IsGrounded)
            MovementActions.InvokeAction("isGrounded");
        else if (!IsGrounded && lastFrame != IsGrounded)
            MovementActions.InvokeAction("isNotGrounded");
    }
}
