using Godot;

public partial class PlayerItemInteractor : Node
{
    [Export] private InputActions _inputActions;
    [Export] private PlayerHotbar _playerHotbar;
    [Export] private CharacterBody3D _player;
    [Export] private RayCast3D _rayOfLook;
    [Export] private float _dropForce = 6.0f;

    public override void _Ready()
    {
        _inputActions.InteractKeyDown += OnInteractKeyPressed;
        _inputActions.DropKeyDown += OnDropKeyPressed;
    }

    public override void _ExitTree()
    {
        _inputActions.InteractKeyDown -= OnInteractKeyPressed;
        _inputActions.DropKeyDown -= OnDropKeyPressed;
    }

    private void OnInteractKeyPressed()
    {
        if (!_rayOfLook.IsColliding())
            return;

        if (_rayOfLook.GetCollider() is not Item item)
            return;

        if (IsItemCanPickup(item))
            PickupItem(item);
    }

    private bool IsItemCanPickup(Item item)
    {
        return item.IsInGroup("Item:Pickupble");
    }

    private void PickupItem(Item item)
    {
        if (!_playerHotbar.IsHotbarSlotEmpty(item.SlotType))
        {
            Item equppedItem = _playerHotbar.GetItemFromSlot(item.SlotType);
            DropItem(equppedItem);
        }

        GlobalRpcFunctions.Instance.RemoveGroup(item.GetPath(), "Item:Pickupble");

        item.Freeze = true;
        item.LinearVelocity = Vector3.Zero;

        CollisionShape3D collisionShape = item.GetNode<CollisionShape3D>("Hitbox");
        collisionShape.Disabled = true;
        _playerHotbar.PlaceItem(item);
    }

    private void OnDropKeyPressed()
    {
        if (_playerHotbar.IsActiveSlotEmpty())
            return;

        Item item = _playerHotbar.GetActiveItem();
        DropItem(item);
    }

    private void DropItem(Item item)
    {
        GlobalRpcFunctions.Instance.AddGroup(item.GetPath(), "Item:Pickupble");

        Vector3 position = item.GlobalPosition;
        Vector3 rotation = item.GlobalRotation;

        item.Position = position;
        item.Rotation = rotation;

        item.Freeze = false;
        item.LinearVelocity = Vector3.Zero;
        item.AngularVelocity = Vector3.Zero;

        Vector3 lookDirection = -_rayOfLook.GlobalTransform.Basis.Z;
        Vector3 dropImpulse = lookDirection * _dropForce;
        item.ApplyCentralImpulse(dropImpulse);

        CollisionShape3D collisionShape = item.GetNode<CollisionShape3D>("Hitbox");
        collisionShape.Disabled = false;

        _playerHotbar.AbortItem(item);
    }
}
