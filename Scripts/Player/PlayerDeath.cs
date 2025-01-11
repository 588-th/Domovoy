using Godot;

public partial class PlayerDeath : Node
{
    [Export] private Node _player;
    [Export] private CharacterBody3D _playerBody;
    [Export] private RayCast3D _rayGround;
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

        RandomNumberGenerator rng = new RandomNumberGenerator();
        rng.Randomize();
        float randomYRotation = Mathf.DegToRad(rng.RandfRange(-30, 30));

        humanCorpse.Rotation = new Vector3(humanCorpse.Rotation.X, _playerBody.Rotation.Y + randomYRotation, 0);
        humanCorpse.Position = GetCorpsePosition();

        GetTree().GetFirstNodeInGroup("Holder:Props").AddChild(humanCorpse, true);

        _player.GetParent().RemoveChild(_player);
        _player.QueueFree();

        GameEvent.Instance.InvokePlayerDied(playerID);
    }


    private Vector3 GetCorpsePosition()
    {
        Vector3 spawnPosition = _playerBody.Position;

        if (_rayGround.IsColliding())
            spawnPosition = _rayGround.GetCollisionPoint();

        return spawnPosition;
    }
}
