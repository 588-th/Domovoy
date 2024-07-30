public abstract class MovementState
{
    protected PlayerMovement _playerMovement;
    protected MovementStateMachine _movementStateMachine;

    protected MovementState(PlayerMovement playerMovement, MovementStateMachine movementStateMachine)
    {
        _playerMovement = playerMovement;
        _movementStateMachine = movementStateMachine;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void LogicUpdate()
    {

    }

    public virtual void PhysicsUpdate(double delta)
    {

    }
}
