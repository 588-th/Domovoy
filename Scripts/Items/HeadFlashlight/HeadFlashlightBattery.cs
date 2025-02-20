using Godot;

public partial class HeadFlashlightBattery : Node
{
    [Export] private HeadFlashlightToggleLight _headFlashlightToggleLight;
    [Export] public float MaxBatteryLife = 60f;
    [Export] public float DrainRate = 1f;

    private float _currentBatteryLife;

    public override void _Process(double delta)
    {
        if (!_headFlashlightToggleLight.IsFlashlightEnabled)
            return;

        Drain((float)delta);
    }

    public float GetBatteryLife()
    {
        return _currentBatteryLife;
    }

    public void SetBatteryLife(float batterLife)
    {
        if (MaxBatteryLife <= batterLife)
            _currentBatteryLife = batterLife;
    }

    public void Drain(float delta)
    {
        _currentBatteryLife -= DrainRate * delta;
        _currentBatteryLife = Mathf.Max(_currentBatteryLife, 0);
    }

    public bool IsDepleted()
    {
        return _currentBatteryLife <= 0;
    }
}
