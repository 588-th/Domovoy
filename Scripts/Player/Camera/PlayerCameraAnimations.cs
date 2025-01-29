using Godot;

public partial class PlayerCameraAnimations : Node
{
    [Export] private MovementActions _movementActions;
    [Export] private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _movementActions.IsIdleState += PlayIdleAnimation;
        _movementActions.IsWalkState += PlayWalkingAnimation;
        _movementActions.IsJumpState += PlayJumpAnimation;
        _movementActions.IsSneakState += PlaySneakAnimation;
    }

    public override void _ExitTree()
    {
        _movementActions.IsIdleState -= PlayIdleAnimation;
        _movementActions.IsWalkState -= PlayWalkingAnimation;
        _movementActions.IsJumpState -= PlayJumpAnimation;
        _movementActions.IsSneakState -= PlaySneakAnimation;
    }

    private void PlayIdleAnimation()
    {
        _animationPlayer.Play("IDLE");
    }

    private void PlayWalkingAnimation()
    {
        _animationPlayer.Play("WALK");
    }

    private void PlayJumpAnimation()
    {
        _animationPlayer.Play("JUMP");
    }

    private void PlaySneakAnimation()
    {
        _animationPlayer.Play("SNEAK");
    }
}
