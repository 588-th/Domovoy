using Godot;

public partial class UIHUDItem : Node
{
    [Export] private HUDParameters _hudParameters;
    [Export] private Label _labelItem;

    public override void _Ready()
    {
        _hudParameters.PlayerHoldingItemUpdated += UpdateItemData;
    }

    public override void _ExitTree()
    {
        _hudParameters.PlayerHoldingItemUpdated -= UpdateItemData;
    }

    private void UpdateItemData()
    {
        _labelItem.Text = _hudParameters.PlayerHoldingItem;
    }
}
