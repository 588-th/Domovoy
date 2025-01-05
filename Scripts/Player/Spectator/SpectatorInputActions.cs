using Godot;
using System;

public partial class SpectatorInputActions : Node
{
    public Action EscapeKeyDown { get; set; }
    public Action SpectateNextPlayerDown { get; set; }
    public Action SpectatePreviousPlayerDown { get; set; }

    public Action EscapeKeyUp { get; set; }
    public Action SpectateNextPlayerUp { get; set; }
    public Action SpectatePreviousPlayerUp { get; set; }

    public override void _Process(double delta)
    {
        ProcessInput();
    }

    private void ProcessInput()
    {
        ProcessKeyAction("escape", EscapeKeyDown, EscapeKeyUp);
        ProcessKeyAction("spectateNextPlayer", SpectateNextPlayerDown, SpectateNextPlayerUp);
        ProcessKeyAction("spectatePreviousPlayer", SpectatePreviousPlayerDown, SpectatePreviousPlayerUp);
    }

    private void ProcessKeyAction(string actionName, Action actionDown, Action actionUp)
    {
        if (Input.IsActionJustPressed(actionName))
            actionDown?.Invoke();
        else if (Input.IsActionJustReleased(actionName)) 
            actionUp?.Invoke();
    }
}
