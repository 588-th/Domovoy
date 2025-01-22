using Godot;

public class WalkState : MovementState
{
    public WalkState(PlayerMovement playerMovement) : base(playerMovement) { }

    public override void Enter()
    {
        base.Enter();

        _playerMovement.MovementActions.InvokeAction("isWalkState");
        _playerMovement.InputActions.JumpKeyDown += OnJumpKeyDown;
        _playerMovement.InputActions.CrouchKeyUp += OnCrouchUp;
        _playerMovement.InputActions.CrouchKeyDown += OnCrouchDown;
        _playerMovement.MovementActions.IsNotGrounded += OnNotGround;

        _playerMovement.PlayerMovementParameters.CurrentSpeed = _playerMovement.PlayerMovementParameters.WalkSpeed;
        _playerMovement.PlayerMovementParameters.CurrentAcceleration = _playerMovement.PlayerMovementParameters.OnGroundAcceleration;
    }

    public override void Exit()
    {
        base.Exit();

        _playerMovement.MovementActions.InvokeAction("isNotWalkState");
        _playerMovement.InputActions.JumpKeyDown -= OnJumpKeyDown;
        _playerMovement.InputActions.CrouchKeyUp -= OnCrouchUp;
        _playerMovement.InputActions.CrouchKeyDown -= OnCrouchDown;
        _playerMovement.MovementActions.IsNotGrounded -= OnNotGround;
    }

    public override void Update(double delta)
    {
        base.Update(delta);

        CheckInputVector();
        Move(delta);
    }

    private void Move(double delta)
    {
        Vector3 velocity = _playerMovement.PlayerBody.Velocity;
        Vector3 inputVector = _playerMovement.InputVector.Vector;
        Vector3 direction = (_playerMovement.PlayerBody.Transform.Basis * inputVector).Normalized();

        Vector3 targetVelocity = direction == Vector3.Zero
            ? Vector3.Zero
            : new Vector3(direction.X * _playerMovement.PlayerMovementParameters.CurrentSpeed, 0, direction.Z * _playerMovement.PlayerMovementParameters.CurrentSpeed);

        velocity = velocity.MoveToward(targetVelocity, (float)(_playerMovement.PlayerMovementParameters.OnGroundAcceleration * delta));
        _playerMovement.PlayerBody.Velocity = velocity;
    }

    private void CheckInputVector()
    {
        if (_playerMovement.InputVector.Vector == Vector3.Zero)
            _playerMovement.ChangeState(_playerMovement.IdleState);
    }

    private void OnJumpKeyDown()
    {
        _playerMovement.ChangeState(_playerMovement.JumpState);
    }
    
    private void OnNotGround()
    {
        _playerMovement.ChangeState(_playerMovement.JumpState);
    }

    private void OnCrouchDown()
    {
        _playerMovement.PlayerMovementParameters.CurrentSpeed = _playerMovement.PlayerMovementParameters.CrouchSpeed;
    }

    private void OnCrouchUp()
    {
        _playerMovement.PlayerMovementParameters.CurrentSpeed = _playerMovement.PlayerMovementParameters.WalkSpeed;
    }
}
