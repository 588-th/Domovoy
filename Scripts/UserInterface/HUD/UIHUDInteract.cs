using Godot;

public partial class UIHUDInteract : Node
{
    [Export] private HUDParameters _hudParameters;
    [Export] private Label _labelInteract;

    public override void _Ready()
    {
        _hudParameters.PlayerInteractDataUpdated += UpdateInteractData;
    }

    public override void _ExitTree()
    {
        _hudParameters.PlayerInteractDataUpdated -= UpdateInteractData;
    }

    private void UpdateInteractData()
    {
        _labelInteract.Text = _hudParameters.PlayerInteract;
    }
}
