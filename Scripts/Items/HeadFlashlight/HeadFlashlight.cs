using Godot;

public partial class HeadFlashlight : Item
{
    [Export] private HeadFlashlightToggleLight _headFlashlightToggleLight;
    [Export] private HeadFlashlightBattery _headFlashlightBattery;

    public override void BindOnHandActions(InputActions inputAction)
    {
        inputAction.AttackKeyDown += _headFlashlightToggleLight.ToggleFlashlight;
    }

    public override void UnbindOnHandActions(InputActions inputAction)
    {
        inputAction.AttackKeyDown -= _headFlashlightToggleLight.ToggleFlashlight;
    }

    public override void BindEquipActions(InputActions inputAction)
    {
        inputAction.ToggleFlashlightDown += _headFlashlightToggleLight.ToggleFlashlight;
    }

    public override void UnbindEquipActions(InputActions inputAction)
    {
        inputAction.ToggleFlashlightDown -= _headFlashlightToggleLight.ToggleFlashlight;
    }

    public override string GetItemInfo()
    {
        return $"{Name} Battery:{_headFlashlightBattery.GetBatteryLife}";
    }
}
