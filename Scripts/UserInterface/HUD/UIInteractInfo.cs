using Godot;

public partial class UIInteractInfo : Node
{
    [Export] private HUDParameters _hudParameters;
    [Export] private Label _labelInteractInfo;

    public override void _Ready()
    {
        _hudParameters.PlayerInteractDataUpdated += UpdateInteractInfo;
    }

    public override void _ExitTree()
    {
        _hudParameters.PlayerInteractDataUpdated -= UpdateInteractInfo;
    }

    private void UpdateInteractInfo()
    {
        _labelInteractInfo.Text = _hudParameters.PlayerInteract;
    }
}
