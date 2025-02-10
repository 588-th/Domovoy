using Godot;

public partial class PlayerInputVector : Node
{
    [Export] private InputVector _inputVector;
    [Export] private UIMenuPause _UIMenuPause;

    public override void _Process(double delta)
    {
        if (_UIMenuPause.IsPaused)
        {
            _inputVector.Vector = new();
            return;
        }

        CalculateInputVector();
    }

    private void CalculateInputVector()
    {
        Vector2 InputDirection = Input.GetVector("left", "right", "up", "down");
        _inputVector.Vector = new Vector3(InputDirection.X, 0f, InputDirection.Y).Normalized();
    }
}
