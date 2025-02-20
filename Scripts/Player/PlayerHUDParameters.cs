using Godot;

public partial class PlayerHUDParameters : Node
{
    [Export] private HUDParameters _hudParameters;

    [Export] private PlayerHealth _playerHealth;
    [Export] private PlayerItemEquipper _playerItemEquipper;
    [Export] private RayCast3D _rayOfLook;
    [Export] private RayCast3D _rayOfAttack;

    private Item _rememberItem = new();

    public override void _Ready()
    {
        if (_playerHealth != null)
        {
            _playerHealth.PlayerHealthChanged += TransferHealthData;
            TransferHealthData();
            TransferMaxHealthData();
        }

        if (_playerItemEquipper != null)
        {
            _playerItemEquipper.ActiveSlotChanged += TransferItemData;
            _playerItemEquipper.ActiveSlotChanged += SubToItemUsed;
        }
    }

    public override void _ExitTree()
    {
        if (_playerHealth != null)
        {
            _playerHealth.PlayerHealthChanged -= TransferHealthData;
        }

        if (_playerItemEquipper != null)
        {
            _playerItemEquipper.ActiveSlotChanged -= TransferItemData;
            _playerItemEquipper.ActiveSlotChanged -= SubToItemUsed;
        }
    }

    public override void _Process(double delta)
    {
        if (_rayOfLook != null)
            TransferInteractData();

        if (_rayOfAttack != null)
            TransfetAttackData();
    }

    private void TransferHealthData()
    {
        _hudParameters.PlayerHealth = _playerHealth.GetCurrentHealth();
    }

    private void TransferMaxHealthData()
    {
        _hudParameters.PlayerHealthMax = _playerHealth.GetMaxHealth();
    }

    private void TransferItemData(Item item)
    {
        _hudParameters.PlayerHoldingItem = "";
        _hudParameters.PlayerHoldingItem = item?.GetItemInfo();
    }

    private void SubToItemUsed(Item item)
    {
        if (item == null)
            return;

        _rememberItem.ItemUsed -= () => TransferItemData(_rememberItem);
        _rememberItem = item;
        _rememberItem.ItemUsed += () => TransferItemData(_rememberItem);
    }

    private void TransferInteractData()
    {
        _hudParameters.PlayerInteract = "";

        if (!_rayOfLook.IsColliding())
            return;

        Node collider = _rayOfLook.GetCollider() as Node;
        if (collider == null)
            return;

        if (collider.IsInGroup("Item:Pickupble") && collider is Item item)
            _hudParameters.PlayerInteract = $"[E] Pickup {item.Name}";
        else if (collider.IsInGroup("Hitbox:Player") && _playerItemEquipper.GetHoldingItem() is Medkit)
            _hudParameters.PlayerInteract = "[Hold right button] to heal teammate";
    }

    private void TransfetAttackData()
    {
        _hudParameters.PlayerAttack = "";

        if (!_rayOfAttack.IsColliding())
            return;

        Node collider = _rayOfAttack.GetCollider() as Node;
        if (collider == null)
            return;

        if (collider.IsInGroup("Hitbox:Player"))
            _hudParameters.PlayerAttack = "Attack Human";
    }
}
