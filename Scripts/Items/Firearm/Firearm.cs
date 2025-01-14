using Godot;

public partial class Firearm : Item
{
    [Export] private FirearmParameters _firearmParameters;
    [Export] private FirearmShoot _firearmShoot;
    [Export] private FirearmReload _firearmReload;
    [Export] private FirearmToggleAutomatic _firearmToggleAutomatic;
    [Export] private FirearmToggleLaser _firearmToggleLaser;

    public override void BindOnHandActions(InputActions inputAction)
    {
        inputAction.AttackKeyDown += _firearmShoot.OnAttackKeyDown;
        inputAction.AttackKeyUp += _firearmShoot.OnAttackKeyUp;
        inputAction.ReloadKeyDown += _firearmReload.Reload;
        inputAction.ToggleFirearmAutomaticDown += _firearmToggleAutomatic.ToggleAutomatic;
        inputAction.ToggleFirearmLaserDown += _firearmToggleLaser.ToggleLaser;
    }

    public override void UnbindOnHandActions(InputActions inputAction)
    {
        inputAction.AttackKeyDown -= _firearmShoot.OnAttackKeyDown;
        inputAction.AttackKeyUp -= _firearmShoot.OnAttackKeyUp;
        inputAction.ReloadKeyDown -= _firearmReload.Reload;
        inputAction.ToggleFirearmAutomaticDown -= _firearmToggleAutomatic.ToggleAutomatic;
        inputAction.ToggleFirearmLaserDown -= _firearmToggleLaser.ToggleLaser;
    }

    public override string GetItemInfo()
    {
        return $"{_firearmParameters.CurrentBulletsInClip}/{_firearmParameters.CurrentClips} {Name}";
    }
}
