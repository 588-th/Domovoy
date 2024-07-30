public class MovementStateMachine
{
    public MovementState CurrentState { get; private set; }

    public StandState StandState { get; private set; }
    public CrouchState CrouchState { get; private set; }
    public JumpState JumpState { get; private set; }
    public JumpCrouchState JumpCrouchState { get; private set; }
    public SlideState SlideState { get; private set; }
    public WallrunState WallrunState { get; private set; }

    public MovementStateMachine(PlayerMovement playerMovement)
    {
        StandState = new StandState(playerMovement, this);
        CrouchState = new CrouchState(playerMovement, this);
        JumpState = new JumpState(playerMovement, this);
        JumpCrouchState = new JumpCrouchState(playerMovement, this);
        SlideState = new SlideState(playerMovement, this);
        WallrunState = new WallrunState(playerMovement, this);

        CurrentState = StandState;
        CurrentState.Enter();
    }

    public void ChangeState(MovementState newState)
    {
        CurrentState.Exit();
        CurrentState = newState;
        CurrentState.Enter();
    }
}
