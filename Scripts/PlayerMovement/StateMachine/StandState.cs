using Godot;

public class StandState : MovementState
{
    public StandState(PlayerMovement playerMovement, MovementStateMachine movementStateMachine)
        : base(playerMovement, movementStateMachine) { }

    public override void Enter()
    {
        base.Enter();

        _playerMovement.PlayerMovementStats.CurrentSpeed = _playerMovement.PlayerMovementStats.StandForwardSpeed;
        _playerMovement.PlayerMovementStats.CurrentAcceleration = _playerMovement.PlayerMovementStats.OnGroundAcceleration;
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
        Move(delta);
    }

    private void Move(double delta)
    {
        Vector3 velocity = _playerMovement.Velocity;

        // Add the gravity.
        if (!_playerMovement.IsOnFloor())
            velocity.Y -= _playerMovement.PlayerMovementStats.Gravity * (float)delta;

        // Handle Jump.
        if (Input.IsActionJustPressed("jump") && _playerMovement.IsOnFloor())
            velocity.Y = _playerMovement.PlayerMovementStats.JumpSpeed;

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
        Vector3 direction = (_playerMovement.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();

        if (direction != Vector3.Zero)
        {
            float targetHorizontalVelocity = direction.X * _playerMovement.PlayerMovementStats.CurrentSpeed;
            float targetVerticalVelocity = direction.Z * _playerMovement.PlayerMovementStats.CurrentSpeed;

            velocity.X = Mathf.MoveToward(_playerMovement.Velocity.X, targetHorizontalVelocity, _playerMovement.PlayerMovementStats.CurrentAcceleration);
            velocity.Z = Mathf.MoveToward(_playerMovement.Velocity.Z, targetVerticalVelocity, _playerMovement.PlayerMovementStats.CurrentAcceleration);


        }
        else
        {
            velocity.X = Mathf.MoveToward(_playerMovement.Velocity.X, 0, _playerMovement.PlayerMovementStats.CurrentSpeed);
            velocity.Z = Mathf.MoveToward(_playerMovement.Velocity.Z, 0, _playerMovement.PlayerMovementStats.CurrentSpeed);
        }

        _playerMovement.ChangeVelocity(velocity);
        _playerMovement.Velocity = velocity;
        _playerMovement.MoveAndSlide();
    }
}
