using Godot;

public partial class UIItemInfo : Node
{
    [Export] private HUDParameters _hudParameters;
    [Export] private Label _labelInteractInfo;

    public override void _Ready()
    {
        _hudParameters.PlayerHoldingItemUpdated += UpdateItemInfo;
    }

    public override void _ExitTree()
    {
        _hudParameters.PlayerHoldingItemUpdated -= UpdateItemInfo;
    }

    private void UpdateItemInfo()
    {
        _labelInteractInfo.Text = _hudParameters.PlayerHoldingItem;
    }
}
