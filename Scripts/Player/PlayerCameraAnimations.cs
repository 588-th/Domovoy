using Godot;

public partial class PlayerCameraAnimations : Node
{
    [Export] private MovementActions _movementActions;
    [Export] private AnimationPlayer _animationPlayer;

    public override void _Ready()
    {
        _movementActions.IsWalkState += PlayWalkingAnimation;
        _movementActions.IsNotWalkState += StopAnimations;
    }

    public override void _ExitTree()
    {
        _movementActions.IsWalkState -= PlayWalkingAnimation;
        _movementActions.IsNotWalkState -= StopAnimations;
    }

    private void PlayWalkingAnimation()
    {
        _animationPlayer.Play("WALK");
    }

    private void StopAnimations()
    {
        _animationPlayer.Play("RESET");
        _animationPlayer.Stop();
    }
}
