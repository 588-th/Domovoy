using Godot;

public class IdleState : MovementState
{
    public IdleState(PlayerMovement playerMovement) : base(playerMovement) { }

    private bool _isSneakPressed;

    public override void Enter()
    {
        base.Enter();

        _playerMovement.InputActions.SneakKeyDown += OnSneaKeyToggle;
        _playerMovement.InputActions.SneakKeyUp += OnSneaKeyToggle;
        _playerMovement.InputActions.JumpKeyDown += OnJumpKeyDown;
        _playerMovement.MovementActions.IsNotGrounded += OnNotGround;
        _playerMovement.MovementActions.InvokeAction("isIdleState");
    }

    public override void Exit()
    {
        base.Exit();

        _playerMovement.InputActions.SneakKeyDown -= OnSneaKeyToggle;
        _playerMovement.InputActions.SneakKeyUp -= OnSneaKeyToggle;
        _playerMovement.InputActions.JumpKeyDown -= OnJumpKeyDown;
        _playerMovement.MovementActions.IsNotGrounded -= OnNotGround;
    }

    public override void Update(double delta)
    {
        base.Update(delta);

        CheckInutVector();
        Move(delta);
    }

    public override void ExitActionInvoke()
    {
        _playerMovement.MovementActions.InvokeAction("isNotIdleState");
    }

    private void CheckInutVector()
    {
        if (_playerMovement.InputVector.Vector == Vector3.Zero)
            return;

        if (_isSneakPressed)
            _playerMovement.ChangeState(_playerMovement.SneakState);
        else
            _playerMovement.ChangeState(_playerMovement.WalkState);
    }

    private void Move(double delta)
    {
        Vector3 velocity = _playerMovement.PlayerBody.Velocity;
        Vector3 targetVelocity = Vector3.Zero;

        velocity = velocity.MoveToward(targetVelocity, (float)(_playerMovement.CurrentAcceleration * delta));
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

    private void OnSneaKeyToggle()
    {
        _isSneakPressed = !_isSneakPressed;
    }
}
