using Godot;
using System.Collections.Generic;

public partial class PlayerInputKeys : Node
{
    [Export] private InputActions _inputActions;
    [Export] private UIMenuPause _UIMenuPause;

    private readonly List<string> _inputs = new()
    {
        "up", 
        "down", 
        "left", 
        "right",
        "primarySlot", 
        "secondarySlot", 
        "tertiarySlot",
        "quaternarySlot",
        "sneak",
        "crouch", 
        "jump",
        "interact",
        "drop",
        "attack",
        "alternative",
        "reload",
        "toggleMicrophone",
        "toggleMonsterVision",
        "toggleFirearmAutomatic",
        "toggleFirearmLaser",
        "toggleFlashlight",
        "spectateNextPlayer",
        "spectatePreviousPlayer",
    };

    private readonly List<string> _menuInputs = new()
    {
        "escape"
    };

    public override void _Process(double delta)
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        foreach (var input in _menuInputs)
        {
            if (Input.IsActionJustPressed(input))
                _inputActions.InvokeAction(input, true);
            else if (Input.IsActionJustReleased(input))
                _inputActions.InvokeAction(input, false);
        }

        if (_UIMenuPause.IsPaused)
            return;

        foreach (var input in _inputs)
        {
            if (Input.IsActionJustPressed(input))
                _inputActions.InvokeAction(input, true);
            else if (Input.IsActionJustReleased(input))
                _inputActions.InvokeAction(input, false);
        }
    }
}
