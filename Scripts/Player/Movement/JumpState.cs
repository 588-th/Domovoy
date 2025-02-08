using Godot;

public class JumpState : MovementState
{
    private int _jumpCount = 0;

    public JumpState(PlayerMovement playerMovement) : base(playerMovement) { }

    public override void Enter()
    {
        base.Enter();

        _playerMovement.InputActions.JumpKeyDown += OnJumpKeyDown;
        _playerMovement.MovementActions.IsGrounded += OnGround;
        _playerMovement.MovementActions.InvokeAction("isJumpState");

        _playerMovement.CurrentForceJump = _playerMovement.ForceJump;
        _playerMovement.CurrentSpeed = _playerMovement.SpeedJump;
        _playerMovement.CurrentAcceleration = _playerMovement.AccelerationOnAir;

        if (_playerMovement.IsGrounded)
            Jump();
    }

    public override void Exit()
    {
        base.Exit();

        _playerMovement.InputActions.JumpKeyDown -= OnJumpKeyDown;
        _playerMovement.MovementActions.IsGrounded -= OnGround;
    }

    public override void Update(double delta)
    {
        base.Update(delta);

        Move(delta);
        Fall(delta);
    }

    public override void ExitActionInvoke()
    {
        _playerMovement.MovementActions.InvokeAction("isNotJumpState");
    }

    private void Move(double delta)
    {
        Vector3 velocity = _playerMovement.PlayerBody.Velocity;
        Vector3 inputVector = _playerMovement.InputVector.Vector;
        Vector3 direction = (_playerMovement.PlayerBody.Transform.Basis * inputVector).Normalized();

        Vector3 targetVelocity = Vector3.Zero;
        targetVelocity.Y = velocity.Y;

        if (direction != Vector3.Zero)
        {
            targetVelocity.X = direction.X * _playerMovement.CurrentSpeed;
            targetVelocity.Z = direction.Z * _playerMovement.CurrentSpeed;

            velocity = velocity.MoveToward(targetVelocity, (float)(_playerMovement.CurrentAcceleration * delta));
        }

        _playerMovement.PlayerBody.Velocity = velocity;
    }

    private void Fall(double delta)
    {
        Vector3 velocity = _playerMovement.PlayerBody.Velocity;
        velocity.Y -= _playerMovement.Gravity * (float)delta;
        _playerMovement.PlayerBody.Velocity = velocity;
    }

    private void Jump()
    {
        _jumpCount++;
        Vector3 velocity = _playerMovement.PlayerBody.Velocity;
        _playerMovement.PlayerBody.Velocity = new Vector3(velocity.X, 0f, velocity.Z);
        velocity.Y = _playerMovement.CurrentForceJump;
        _playerMovement.PlayerBody.Velocity = velocity;
    }

    private void OnJumpKeyDown()
    {
        if (_jumpCount < _playerMovement.MaxJumps)
        {
            Jump();
        }
    }

    private void OnGround()
    {
        _jumpCount = 0;
        _playerMovement.ChangeState(_playerMovement.IdleState);
    }
}
