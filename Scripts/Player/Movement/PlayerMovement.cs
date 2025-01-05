using Godot;
using System;

public partial class PlayerMovement : Node
{
    [Export] public InputActions InputActions;
    [Export] public InputVector InputVector;
    [Export] public CharacterBody3D PlayerBody;
    [Export] public AudioPlayer AudioPlayer;

    [Export] public PlayerMovementParameters PlayerMovementParameters { get; private set; }
    public MovementState CurrentState { get; private set; }
    public IdleState IdleState { get; private set; }
    public WalkState WalkState { get; private set; }
    public JumpState JumpState { get; private set; }

    public Action IsGrounded;
    public Action IsNotGrounded;

    public Action IsIdling;
    public Action IsWalking;
    public Action IsNotWalking;
    public Action IsJumping;

    public bool Grounded { get; private set; }

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

    private void InitializationFields()
    {
        IdleState = new IdleState(this);
        WalkState = new WalkState(this);
        JumpState = new JumpState(this);

        CurrentState = JumpState;
        CurrentState.Enter();
    }

    private void CheckGround()
    {
        bool lastFrame = Grounded;
        Grounded = PlayerBody.IsOnFloor();

        if (Grounded && lastFrame != Grounded)
            IsGrounded?.Invoke();
        else if (!Grounded && lastFrame != Grounded)
            IsNotGrounded?.Invoke();
    }
}
