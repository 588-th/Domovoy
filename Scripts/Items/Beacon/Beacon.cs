using Godot;

public partial class Beacon : Item
{
    [Export] private BeaconPlace _beaconPlace;

    public override void BindOnHandActions(InputActions inputHandler)
    {
        inputHandler.AttackKeyDown += _beaconPlace.Place;
    }

    public override void UnbindOnHandActions(InputActions inputHandler)
    {
        inputHandler.AttackKeyDown -= _beaconPlace.Place;
    }

    public override string GetItemInfo()
    {
        return $"Beacon";
    }
}
