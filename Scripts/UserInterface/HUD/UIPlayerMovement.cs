using Godot;
using System;

public partial class UIPlayerMovement : Control
{
    [Export] private PlayerMovement _playerMovement;
    [Export] private RichTextLabel _stateLabel;
    [Export] private RichTextLabel _velocityLabel;
    [Export] private RichTextLabel _jumpForceLabel;

    private Vector3 _velocity;
    private Vector3 _roundedVelocity;

    public override void _Ready()
    {
        Show();
    }

	public override void _Process(double delta)
	{
        //UpdateUI();
    }

    private void UpdateUI()
    {
        _stateLabel.Text = "State: " + _playerMovement.CurrentState;
        _velocity = _playerMovement.PlayerBody.Velocity;
        _roundedVelocity = new((float)Math.Round(_velocity.X, 2), (float)Math.Round(_velocity.Y, 2), (float)Math.Round(_velocity.Z, 2));
        _velocityLabel.Text = "Velocity: " + _roundedVelocity;
        _jumpForceLabel.Text = "JumpForce: " + _playerMovement.PlayerMovementParameters.CurrentJumpForce;
    }
}
