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
    }

    public override void _ExitTree()
    {
        _movementActions.IsIdleState -= PlayIdleAnimation;
        _movementActions.IsWalkState -= PlayWalkingAnimation;
        _movementActions.IsJumpState -= PlayJumpAnimation;
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
}
