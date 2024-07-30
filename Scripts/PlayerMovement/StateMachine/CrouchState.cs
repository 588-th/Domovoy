public class CrouchState : MovementState
{
    public CrouchState(PlayerMovement playerMovement, MovementStateMachine movementStateMachine)
        : base(playerMovement, movementStateMachine) { }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void LogicUpdate()
    {
        base.LogicUpdate();
    }

    public override void PhysicsUpdate(double delta)
    {
        base.PhysicsUpdate(delta);
    }
}
