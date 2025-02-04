using Godot;

public partial class Beacon : Item
{
    [Export] private BeaconPlace _beaconPlace;
    [Export] private BeaconToggleRadius _beaconToggleRadius;

    public override void BindOnHandActions(InputActions inputHandler)
    {
        inputHandler.AttackKeyDown += _beaconPlace.Place;
        inputHandler.AlternativeKeyDown += _beaconToggleRadius.ToggleRadius;
    }

    public override void UnbindOnHandActions(InputActions inputHandler)
    {
        inputHandler.AttackKeyDown -= _beaconPlace.Place;
        inputHandler.AlternativeKeyDown -= _beaconToggleRadius.ToggleRadius;
    }

    public override string GetItemInfo()
    {
        return $"Beacon";
    }
}
