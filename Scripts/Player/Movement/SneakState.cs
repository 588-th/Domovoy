using Godot;
using System;

public partial class SneakState : MovementState
{
    public SneakState(PlayerMovement playerMovement) : base(playerMovement) { }

    public override void Enter()
    {
        base.Enter();

        _playerMovement.InputActions.SneakKeyUp += OnSneakKeyUp;
        _playerMovement.InputActions.JumpKeyDown += OnJumpKeyDown;
        _playerMovement.MovementActions.IsNotGrounded += OnNotGround;
        _playerMovement.MovementActions.InvokeAction("isSneakState");

        _playerMovement.CurrentSpeed = _playerMovement.SpeedSneak;
        _playerMovement.CurrentAcceleration = _playerMovement.AccelerationOnGround;
    }

    public override void Exit()
    {
        base.Exit();

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

    public override void ExitActionInvoke()
    {
        _playerMovement.MovementActions.InvokeAction("isNotSneakState");
    }

    private void Move(double delta)
    {
        Vector3 velocity = _playerMovement.PlayerBody.Velocity;
        Vector3 inputVector = _playerMovement.InputVector.Vector;
        Vector3 direction = (_playerMovement.PlayerBody.Transform.Basis * inputVector).Normalized();

        Vector3 targetVelocity = direction == Vector3.Zero
            ? Vector3.Zero
            : new Vector3(direction.X * _playerMovement.CurrentSpeed, 0, direction.Z * _playerMovement.CurrentSpeed);

        velocity = velocity.MoveToward(targetVelocity, (float)(_playerMovement.AccelerationOnGround * delta));
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
