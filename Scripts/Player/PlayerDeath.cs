using Godot;

public partial class PlayerDeath : Node
{
    [Export] private Node _player;
    [Export] private PlayerHealth _playerHealth;

    public override void _Ready()
    {
        _playerHealth.PlayerHealthZero += KillPlayer;
    }

    public override void _ExitTree()
    {
        _playerHealth.PlayerHealthZero -= KillPlayer;
    }

    private void KillPlayer()
    {
        long playerID = long.Parse(_player.Name);
        _player.Name += "Crops";
        _player.QueueFree();
        GameEvent.Instance.InvokePlayerDied(playerID);
    }
}
