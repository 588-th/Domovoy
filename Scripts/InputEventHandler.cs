using Godot;
using System;

public partial class InputEventHandler : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
        Vector2 inputDir = Input.GetVector("left", "right", "up", "down");
        int inputY = Convert.ToInt32(Input.IsActionJustPressed("jump"));
        Vector3 direction = new Vector3(inputDir.X, inputY, inputDir.Y).Normalized();
    }

	public Action<MovementState> StateChanged;
}
