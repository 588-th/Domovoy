using Godot;

public partial class PlayerInputVector : Node
{
    [Export] InputVector _inputVector;

    public override void _Process(double delta)
    {
        CalculateInputVector();
    }

    private void CalculateInputVector()
    {
        Vector2 InputDirection = Input.GetVector("left", "right", "up", "down");
        _inputVector.Vector = new Vector3(InputDirection.X, 0f, InputDirection.Y).Normalized();
    }
}
