using Godot;

public partial class Fuse : Item
{
    [Export] private FuseSetting _fuseSetting;

    public override void BindOnHandActions(InputActions inputHandler)
    {
        inputHandler.AttackKeyDown += _fuseSetting.Setting;
        _fuseSetting.FuseUsed += FreeItem;
    }

    public override void UnbindOnHandActions(InputActions inputHandler)
    {
        inputHandler.AttackKeyDown -= _fuseSetting.Setting;
        _fuseSetting.FuseUsed -= FreeItem;
    }

    public override string GetItemInfo()
    {
        return $"Fuse";
    }
}
