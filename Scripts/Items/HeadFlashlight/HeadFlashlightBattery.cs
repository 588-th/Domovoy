using Godot;

public partial class HeadFlashlightBattery : Node
{
    [Export] public float MaxBatteryLife = 100f;
    [Export] public float DrainRate = 5f;

    private float _currentBatteryLife;

    public override void _Ready()
    {
        _currentBatteryLife = MaxBatteryLife;
    }

    public float GetBatteryLife()
    {
        return _currentBatteryLife;
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
