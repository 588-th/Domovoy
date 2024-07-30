using Godot;
using System;

public class TempMovementLogic
{
    private PlayerMovement _playerMovement;

    private void StandState(double delta)
    {
        // Move
        Vector3 velocity = _playerMovement.Velocity;
        Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
        int inputY = Convert.ToInt32(Input.IsActionJustPressed("jump"));
        Vector3 direction = (_playerMovement.Transform.Basis * new Vector3(inputDir.X, inputY, inputDir.Y)).Normalized();
        _playerMovement.PlayerMovementStats.CurrentAcceleration = _playerMovement.PlayerMovementStats.OnGroundAcceleration;
        _playerMovement.PlayerMovementStats.CurrentSpeed = inputDir.Y < 0
            ? _playerMovement.PlayerMovementStats.StandBackwardSpeed
            : _playerMovement.PlayerMovementStats.StandForwardSpeed;
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
        _playerMovement.Velocity = velocity;
        _playerMovement.MoveAndSlide();
    }

    private void CrouchState(double delta)
    {
        // Move
        Vector3 velocity = _playerMovement.Velocity;
        Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
        Vector3 direction = (_playerMovement.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        _playerMovement.PlayerMovementStats.CurrentSpeed = _playerMovement.PlayerMovementStats.CrouchSpeed;
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
        _playerMovement.Velocity = velocity;
        _playerMovement.MoveAndSlide();


        // Crouch
        bool IsCrouchHold = true;
        bool IsCeiling = true;
        if (IsCrouchHold)
        {
            _playerMovement.Scale = new Vector3(1, _playerMovement.PlayerMovementStats.CrouchScaleY, 1);
            _playerMovement.Position = new Vector3(/*some param*/);
        }
        else if (!IsCeiling)
        {
            _playerMovement.Scale = new Vector3(1, 1, 1);
            _playerMovement.Position = new Vector3(/*some param*/);
        }
    }

    private void JumpState(double delta)
    {
        // Move
        Vector3 velocity = _playerMovement.Velocity;
        Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
        Vector3 direction = (_playerMovement.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        _playerMovement.PlayerMovementStats.CurrentSpeed = _playerMovement.PlayerMovementStats.JumpSpeed;
        if (direction != Vector3.Zero)
        {
            float targetHorizontalVelocity = direction.X * _playerMovement.PlayerMovementStats.CurrentSpeed;
            float targetVerticalVelocity = direction.Z * _playerMovement.PlayerMovementStats.CurrentSpeed;

            velocity.X = Mathf.MoveToward(_playerMovement.Velocity.X, targetHorizontalVelocity, _playerMovement.PlayerMovementStats.CurrentAcceleration);
        }


        // Jump
        int maxJumps = 2;
        int jumpCount = 0;
        if (_playerMovement.IsOnFloor() || jumpCount < maxJumps)
        {
            jumpCount++;
            velocity.Y = _playerMovement.PlayerMovementStats.JumpForce;
        }
        if (_playerMovement.IsOnFloor())
        {
            jumpCount = 0;
        }
        if (!_playerMovement.IsOnFloor())
            velocity.Y -= _playerMovement.PlayerMovementStats.Gravity * (float)delta;

        _playerMovement.Velocity = velocity;
        _playerMovement.MoveAndSlide();
    }

    private void JumpCrouch(double delta)
    {
        // Move
        Vector3 velocity = _playerMovement.Velocity;
        Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
        Vector3 direction = (_playerMovement.Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        _playerMovement.PlayerMovementStats.CurrentSpeed = _playerMovement.PlayerMovementStats.JumpSpeed;
        if (direction != Vector3.Zero)
        {
            float targetHorizontalVelocity = direction.X * _playerMovement.PlayerMovementStats.CurrentSpeed;
            float targetVerticalVelocity = direction.Z * _playerMovement.PlayerMovementStats.CurrentSpeed;

            velocity.X = Mathf.MoveToward(_playerMovement.Velocity.X, targetHorizontalVelocity, _playerMovement.PlayerMovementStats.CurrentAcceleration);
        }


        // Jump
        int maxJumps = 2;
        int jumpCount = 0;
        if (_playerMovement.IsOnFloor() || jumpCount < maxJumps)
        {
            jumpCount++;
            velocity.Y = _playerMovement.PlayerMovementStats.JumpForce;
        }
        if (_playerMovement.IsOnFloor())
        {
            jumpCount = 0;
        }
        if (!_playerMovement.IsOnFloor())
            velocity.Y -= _playerMovement.PlayerMovementStats.Gravity * (float)delta;

        _playerMovement.Velocity = velocity;
        _playerMovement.MoveAndSlide();


        // Crouch
        bool IsCrouchHold = true;
        bool IsCeiling = true;
        if (IsCrouchHold)
        {
            _playerMovement.Scale = new Vector3(1, _playerMovement.PlayerMovementStats.CrouchScaleY, 1);
            _playerMovement.Position = new Vector3(/*some param*/);
        }
        else if (!IsCeiling)
        {
            _playerMovement.Scale = new Vector3(1, 1, 1);
            _playerMovement.Position = new Vector3(/*some param*/);
        }
    }

    private void SlideState()
    {
        // Crouch
        bool IsCrouchHold = true;
        bool IsCeiling = true;
        if (IsCrouchHold)
        {
            _playerMovement.Scale = new Vector3(1, _playerMovement.PlayerMovementStats.CrouchScaleY, 1);
            _playerMovement.Position = new Vector3(/*some param*/);
        }
        else if (!IsCeiling)
        {
            _playerMovement.Scale = new Vector3(1, 1, 1);
            _playerMovement.Position = new Vector3(/*some param*/);
        }
    }

    private void WallrunState()
    {

    }
}
