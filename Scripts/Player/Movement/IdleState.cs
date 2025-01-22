using Godot;

public class IdleState : MovementState
{
    public IdleState(PlayerMovement playerMovement) : base(playerMovement) { }

    public override void Enter()
    {
        base.Enter();

        _playerMovement.MovementActions.InvokeAction("isIdleState");
        _playerMovement.InputActions.JumpKeyDown += OnJumpKeyDown;
        _playerMovement.MovementActions.IsNotGrounded += OnNotGround;
    }

    public override void Exit()
    {
        base.Exit();

        _playerMovement.MovementActions.InvokeAction("isNotIdleState");
        _playerMovement.InputActions.JumpKeyDown -= OnJumpKeyDown;
        _playerMovement.MovementActions.IsNotGrounded -= OnNotGround;
    }

    public override void Update(double delta)
    {
        base.Update(delta);
        CheckInutVector();
        Move(delta);
    }

    private void CheckInutVector()
    {
        if (_playerMovement.InputVector.Vector != Vector3.Zero)
            _playerMovement.ChangeState(_playerMovement.WalkState);
    }

    private void Move(double delta)
    {
        Vector3 velocity = _playerMovement.PlayerBody.Velocity;
        Vector3 targetVelocity = Vector3.Zero;

        velocity = velocity.MoveToward(targetVelocity, (float)(_playerMovement.PlayerMovementParameters.CurrentAcceleration * delta));
        _playerMovement.PlayerBody.Velocity = velocity;
    }

    private void OnJumpKeyDown()
    {
        _playerMovement.ChangeState(_playerMovement.JumpState);
    }

    private void OnNotGround()
    {
        _playerMovement.ChangeState(_playerMovement.JumpState);
    }
}
