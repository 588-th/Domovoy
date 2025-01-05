using Godot;

public partial class MedkitHealPlayer : Node
{
    [Export] private Medkit _medkit;
    [Export] private MedkitParameters _medkitParameters;

    public void HealItself()
    {
        if (_medkitParameters.Usages <= 0)
            return;

        PlayerHealth playerHealth = GetSelfPlayerHealth();
        playerHealth.IncreaseHealth(_medkitParameters.HealUnits);
        _medkitParameters.Usages--;
        _medkit.ItemUsed?.Invoke();
    }

    private PlayerHealth GetSelfPlayerHealth()
    {
        var playerHealthMeta = _medkit.HoldingPlayer.GetMeta("PlayerHealth");
        PlayerHealth playerHealth = _medkit.HoldingPlayer.GetNode(playerHealthMeta.ToString()) as PlayerHealth;

        return playerHealth;
    }
}
