using Godot;

public partial class PlayerCamera : Camera3D
{
    [Export] private CharacterBody3D _characterBody3D;
    [Export] private float _sensivity = 0.003f;

    public override void _Ready()
    {
        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _UnhandledInput(InputEvent @event)
    {
        base._UnhandledInput(@event);
        if (@event is InputEventMouseMotion mouseMotion)
        {
            _characterBody3D.RotateY(-mouseMotion.Relative.X * _sensivity);
            RotateX(-mouseMotion.Relative.Y * _sensivity);
            Rotation = new Vector3(Mathf.Clamp(Rotation.X, Mathf.DegToRad(-90), Mathf.DegToRad(90)), Rotation.Y, Rotation.Z);
        }
    }
}
