using Godot;

public partial class HeadFlashlightSettings : Node
{
    [Export] private HeadFlashlightBattery _headFlashlightBattery;

    public override void _Ready()
    {
        SetSettings();
    }

    private void SetSettings()
    {
        _headFlashlightBattery.MaxBatteryLife = SettingsRound.Instance.HeadFlashlightBattery;
        _headFlashlightBattery.SetBatteryLife(SettingsRound.Instance.HeadFlashlightBattery);
    }
}
