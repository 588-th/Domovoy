using Godot;
using System;

public partial class SneakState : MovementState
{
    public SneakState(PlayerMovement playerMovement) : base(playerMovement) { }

    public override void Enter()
    {
        base.Enter();

        _playerMovement.MovementActions.InvokeAction("isSneakState");
        _playerMovement.InputActions.SneakKeyUp += OnSneakKeyUp;
        _playerMovement.InputActions.JumpKeyDown += OnJumpKeyDown;
        _playerMovement.MovementActions.IsNotGrounded += OnNotGround;

        _playerMovement.PlayerMovementParameters.CurrentSpeed = _playerMovement.PlayerMovementParameters.SneakSpeed;
        _playerMovement.PlayerMovementParameters.CurrentAcceleration = _playerMovement.PlayerMovementParameters.OnGroundAcceleration;
    }

    public override void Exit()
    {
        base.Exit();

        _playerMovement.MovementActions.InvokeAction("isNotSneakState");
        _playerMovement.InputActions.SneakKeyUp -= OnSneakKeyUp;
        _playerMovement.InputActions.JumpKeyDown -= OnJumpKeyDown;
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

    private void OnSneakKeyUp()
    {
        _playerMovement.ChangeState(_playerMovement.WalkState);
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
