using Godot;
using System;

public partial class PlayerMonsterNerf : Node
{
    [Export] private PlayerMovement _playerMovement;
    [Export] private PlayerAuraVision _playerAuraVision;
    [Export] private float _speedWalk = 3f;
    [Export] private float _speedSneak = 2f;
    [Export] private float _speedCrouch = 1f;
    [Export] private float _speedJump = 3f;
    [Export] private float _accelerationOnGround = 30f;
    [Export] private float _accelerationOnAir = 20f;

    public override void _Ready()
    {
        GameEvent.Instance.ShieldFixed += Nerf;
    }

    public override void _ExitTree()
    {
        GameEvent.Instance.ShieldFixed -= Nerf;
    }

    private void Nerf()
    {
        _playerMovement.SpeedWalk = _speedWalk;
        _playerMovement.SpeedSneak = _speedSneak;
        _playerMovement.SpeedCrouch = _speedCrouch;
        _playerMovement.SpeedJump = _speedJump;
        _playerMovement.AccelerationOnGround = _accelerationOnGround;
        _playerMovement.AccelerationOnAir = _accelerationOnAir;
        _playerAuraVision.IsVisionEnable = false;
        _playerAuraVision.ToggleOffMonsterVision();
    }
}
