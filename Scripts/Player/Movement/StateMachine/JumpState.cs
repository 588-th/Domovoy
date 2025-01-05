using Godot;

public class JumpState : MovementState
{
    private int _jumpCount = 0;

    public JumpState(PlayerMovement playerMovement) : base(playerMovement) { }

    public override void Enter()
    {
        base.Enter();
        _playerMovement.IsJumping?.Invoke();

        _playerMovement.InputActions.JumpKeyDown += OnJumpKeyDown;
        _playerMovement.IsGrounded += OnGround;

        _playerMovement.PlayerMovementParameters.CurrentSpeed = _playerMovement.PlayerMovementParameters.JumpSpeed;
        _playerMovement.PlayerMovementParameters.CurrentAcceleration = _playerMovement.PlayerMovementParameters.OnAirAcceleration;

        if (_playerMovement.Grounded)
            Jump();
    }

    public override void Exit()
    {
        base.Exit();

        _playerMovement.InputActions.JumpKeyDown -= OnJumpKeyDown;
        _playerMovement.IsGrounded -= OnGround;
    }

    public override void Update(double delta)
    {
        base.Update(delta);
        Move(delta);
        Fall(delta);
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
            targetVelocity.X = direction.X * _playerMovement.PlayerMovementParameters.CurrentSpeed;
            targetVelocity.Z = direction.Z * _playerMovement.PlayerMovementParameters.CurrentSpeed;

            velocity = velocity.MoveToward(targetVelocity, (float)(_playerMovement.PlayerMovementParameters.CurrentAcceleration * delta));
        }

        _playerMovement.PlayerBody.Velocity = velocity;
    }

    private void Fall(double delta)
    {
        Vector3 velocity = _playerMovement.PlayerBody.Velocity;
        velocity.Y -= _playerMovement.PlayerMovementParameters.Gravity * (float)delta;
        _playerMovement.PlayerBody.Velocity = velocity;
    }

    private void Jump()
    {
        _jumpCount++;
        Vector3 velocity = _playerMovement.PlayerBody.Velocity;
        _playerMovement.PlayerBody.Velocity = new Vector3(velocity.X, 0f, velocity.Z);
        velocity.Y = _playerMovement.PlayerMovementParameters.CurrentJumpForce;
        _playerMovement.PlayerBody.Velocity = velocity;

        _playerMovement.AudioPlayer.PlayAudio(_playerMovement.PlayerMovementParameters.JumpAudio);
        if (!_playerMovement.PlayerMovementParameters.PlayAudioOnlyLocaly)
            _playerMovement.AudioPlayer.PlayAudio3DForAll(_playerMovement.PlayerMovementParameters.JumpAudio, false);
    }

    private void OnJumpKeyDown()
    {
        if (_jumpCount < _playerMovement.PlayerMovementParameters.MaxJumps)
        {
            Jump();
        }
    }

    private void OnGround()
    {
        _jumpCount = 0;
        _playerMovement.ChangeState(_playerMovement.WalkState);

        _playerMovement.AudioPlayer.PlayAudio(_playerMovement.PlayerMovementParameters.LandingAudio);
        if (!_playerMovement.PlayerMovementParameters.PlayAudioOnlyLocaly)
            _playerMovement.AudioPlayer.PlayAudio3DForAll(_playerMovement.PlayerMovementParameters.LandingAudio, false);
    }
}
