using Godot;

public partial class PlayerDeath : Node
{
    [Export] private Node _playerRoot;
    [Export] private PlayerHealth _playerHealth;
    [Export] private CharacterBody3D _playerBody;

    [ExportGroup("Spawn Corpse")]
    [Export] private bool _spawnCorpse;
    [Export] private RayCast3D _rayGround;
    [Export] private PackedScene _corpseScene;

    [ExportGroup("Drop Items")]
    [Export] private bool _dropItems;
    [Export] private PlayerHotbar _playerHotbar;

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
        long playerID = long.Parse(_playerRoot.Name);

        DropItems();
        SpawnCorpse(playerID);
        DespawnPlayer();

        GameEvent.Instance.InvokePlayerDied(playerID);
    }

    private void DropItems()
    {
        if (!_dropItems)
            return;

        DropItem(HotbarSlotType.Primary);
        DropItem(HotbarSlotType.Secondary);
        DropItem(HotbarSlotType.Tertiary);
        DropItem(HotbarSlotType.Quaternary);
    }

    private void DropItem(HotbarSlotType hotbarSlotType)
    {
        Item item = _playerHotbar.GetItemFromSlot(hotbarSlotType);
        if (item == null)
            return;

        GlobalRpcFunctions.Instance.AddGroup(item.GetPath(), "Item:Pickupble");

        Vector3 position = item.GlobalPosition;
        Vector3 rotation = new();

        if (_rayGround.IsColliding())
            position = new Vector3(position.X, _rayGround.GetCollisionPoint().Y, position.Z);

        item.Position = position;
        item.Rotation = rotation;

        item.Freeze = false;
        item.LinearVelocity = Vector3.Zero;
        item.AngularVelocity = Vector3.Zero;

        CollisionShape3D collisionShape = item.GetNode<CollisionShape3D>("Hitbox");
        collisionShape.Disabled = false;
        _playerHotbar.AbortItem(item);
    }

    private void SpawnCorpse(long playerID)
    {
        if (!_spawnCorpse)
            return;

        Node3D humanCorpse = _corpseScene.Instantiate<Node3D>();
        humanCorpse.Name = playerID.ToString();
        humanCorpse = SetCorpsePosition(humanCorpse);
        humanCorpse = SetCorpseRotation(humanCorpse);
        GetTree().GetFirstNodeInGroup("Holder:Props").AddChild(humanCorpse, true);
    }

    private Node3D SetCorpsePosition(Node3D humanCorpse)
    {
        Vector3 spawnPosition = _playerBody.Position;
        if (_rayGround.IsColliding())
            spawnPosition = _rayGround.GetCollisionPoint();
        humanCorpse.Position = spawnPosition;

        return humanCorpse;
    }

    private Node3D SetCorpseRotation(Node3D humanCorpse)
    {
        RandomNumberGenerator rng = new();
        rng.Randomize();
        float randomYRotation = Mathf.DegToRad(rng.RandfRange(-30, 30));
        humanCorpse.Rotation = new Vector3(humanCorpse.Rotation.X, _playerBody.Rotation.Y + randomYRotation, 0);

        return humanCorpse;
    }

    private void DespawnPlayer()
    {
        _playerRoot.GetParent().RemoveChild(_playerRoot);
        _playerRoot.QueueFree();
    }
}
