using Godot;

public partial class HumanSettings : Node
{
    [Export] private PlayerHealth _playerHealth;

    public override void _Ready()
    {
        SetSettings();
    }

    private void SetSettings()
    {
        _playerHealth.SetMaxHealth(SettingsRound.Instance.HumanHealth);
        _playerHealth.SetCurrentHealth(SettingsRound.Instance.HumanHealth);
    }
}
