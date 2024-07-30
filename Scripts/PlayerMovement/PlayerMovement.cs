using Godot;

public partial class PlayerMovement : CharacterBody3D
{
    [Export] private Camera3D _camera;

    public PlayerMovementStats PlayerMovementStats;
    private MovementStateMachine _movementStateMachine;

    public void ChangeVelocity(Vector3 newVelocity)
    {
        Velocity = newVelocity;
    }

    public override void _Ready()
    {
        base._Ready();

        PlayerMovementStats = new PlayerMovementStats();
        _movementStateMachine = new MovementStateMachine(this);
    }

    public override void _Process(double delta)
    {
        base._Process(delta);

        _movementStateMachine.CurrentState.LogicUpdate();
    }

    public override void _PhysicsProcess(double delta)
    {
        base._PhysicsProcess(delta);

        _movementStateMachine.CurrentState.PhysicsUpdate(delta);
    }
}
