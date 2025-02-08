public abstract class MovementState
{
    protected PlayerMovement _playerMovement;

    protected MovementState(PlayerMovement playerMovement)
    {
        _playerMovement = playerMovement;
    }

    public virtual void Enter()
    {

    }

    public virtual void Exit()
    {

    }

    public virtual void Update(double delta)
    {

    }

    public virtual void ExitActionInvoke()
    {

    }
}
