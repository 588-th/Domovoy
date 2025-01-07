using Godot;

public partial class UIPauseMenu : Control
{
    [Export] private InputActions _inputActions;
    private bool _isPaused;

    public override void _Ready()
    {
        HidePauseMenu();
        _inputActions.EscapeKeyDown += OnEscapeKeyPressed;
    }

    public override void _ExitTree()
    {
        _inputActions.EscapeKeyDown -= OnEscapeKeyPressed;
    }

    public void ShowPauseMenu()
    {
        _isPaused = true;
        Input.MouseMode = Input.MouseModeEnum.Visible;
        Show();
    }

    public void HidePauseMenu()
    {
        _isPaused = false;
        Input.MouseMode = Input.MouseModeEnum.Captured;
        Hide();
    }

    private void OnEscapeKeyPressed()
    {
        if (_isPaused)
            HidePauseMenu();
        else
            ShowPauseMenu();
    }
}
