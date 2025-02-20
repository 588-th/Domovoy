using Godot;

public partial class MedkitHealOtherPlayer : Node
{
    [Export] private Medkit _medkit;
    [Export] private MedkitParameters _medkitParameters;

    public void HealOtherPlayer()
    {
        if (_medkitParameters.Usages <= 0)
            return;

        if (!CanHealOtherPlayer())
            return;

        PlayerHealth playerHealth = GetOtherPlayerHealth();
        playerHealth.IncreaseHealth(_medkitParameters.HealUnits);
        _medkitParameters.Usages--;
        _medkit.ItemUsed?.Invoke();
    }

    public bool CanHealOtherPlayer()
    {
        var rayOfLookMeta = _medkit.HoldingPlayer.GetMeta("RayOfLook");
        RayCast3D rayOfLook = _medkit.HoldingPlayer.GetNode(rayOfLookMeta.ToString()) as RayCast3D;
        Node collider = rayOfLook.GetCollider() as Node;
        if (collider == null)
            return false;

        if (!collider.IsInGroup("Hitbox:Player"))
            return false;

        PlayerHealth playerHealth = GetOtherPlayerHealth();
        if (playerHealth.GetMaxHealth() == playerHealth.GetCurrentHealth())
            return false;

        return true;
    }

    private PlayerHealth GetOtherPlayerHealth()
    {
        var rayOfLookMeta = _medkit.HoldingPlayer.GetMeta("RayOfLook");
        RayCast3D rayOfLook = _medkit.HoldingPlayer.GetNode(rayOfLookMeta.ToString()) as RayCast3D;
        Node collider = rayOfLook.GetCollider() as Node;

        var playerRootMeta = collider.GetMeta("PlayerRoot");
        Node playerRoot = collider.GetNode(playerRootMeta.ToString());

        var playerHealthMeta = playerRoot.GetMeta("PlayerHealth");
        PlayerHealth playerHealth = playerRoot.GetNode(playerHealthMeta.ToString()) as PlayerHealth;

        return playerHealth;
    }
}
