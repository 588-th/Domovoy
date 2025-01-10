using Godot;

public partial class PlayerDeath : Node
{
    [Export] private Node _player;
    [Export] private CharacterBody3D _playerBody;
    [Export] private PlayerHealth _playerHealth;
    [Export] private PackedScene _humanCorpseScene;

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

        Node3D humanCorpse = _humanCorpseScene.Instantiate<Node3D>();
        humanCorpse.Name = playerID.ToString();
        humanCorpse.Rotation = new Vector3(humanCorpse.Rotation.X, _playerBody.Rotation.Y, 0);
        humanCorpse.Position = new Vector3(_playerBody.Position.X, 0, _playerBody.Position.Z);

        GetTree().GetFirstNodeInGroup("Holder:Props").AddChild(humanCorpse, true);

        _player.GetParent().RemoveChild(_player);

        GameEvent.Instance.InvokePlayerDied(playerID);
    }
}
