using Godot;

public partial class Medkit : Item
{
    [Export] private MedkitParameters _medkitParameters;
    [Export] private MedkitHealPlayer _medkitHealPlayer;
    [Export] private MedkitHealOtherPlayer _medkitHealOtherPlayer;

    public override void BindOnHandActions(InputActions inputHandler)
    {
        inputHandler.AttackKeyDown += _medkitHealPlayer.HealItself;
        inputHandler.AlternativeKeyDown += _medkitHealOtherPlayer.HealOtherPlayer;
    }

    public override void UnbindOnHandActions(InputActions inputHandler)
    {
        inputHandler.AttackKeyDown -= _medkitHealPlayer.HealItself;
        inputHandler.AlternativeKeyDown -= _medkitHealOtherPlayer.HealOtherPlayer;
    }

    public override string GetItemInfo()
    {
        return $"{_medkitParameters.Usages} {_medkitParameters.HealUnits}health {Name}";
    }
}
