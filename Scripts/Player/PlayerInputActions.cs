using Godot;
using System.Collections.Generic;

public partial class PlayerInputActions : Node
{
    [Export] private InputActions _inputActions;

    private readonly List<string> _inputs = new()
    {
        "up", "down", "left", "right",
        "primarySlot", "secondarySlot", "tertiarySlot",
        "crouch", "jump",
        "escape",
        "interact",
        "drop",
        "attack",
        "alternative",
        "reload",
        "toggleMonsterVision",
        "toggleFirearmAutomatic",
        "toggleFirearmLaser",
        "toggleFlashlight",
    };

    public override void _Process(double delta)
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        foreach (var input in _inputs)
        {
            if (Input.IsActionJustPressed(input))
                _inputActions.InvokeAction(input, true);
            else if (Input.IsActionJustReleased(input))
                _inputActions.InvokeAction(input, false);
        }
    }
}
