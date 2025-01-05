using Godot;

public partial class PlayerHUDParameters : Node
{
    [Export] private HUDParameters _HUDParameters;

    [Export] private PlayerHealth _playerHealth;
    [Export] private PlayerItemEquipper _playerItemEquipper;
    [Export] private RayCast3D _rayOfLook;

    private Item _rememberItem = new();

    public override void _Ready()
    {
        _playerHealth.PlayerHealthChanged += TransferHealthData;
        _playerItemEquipper.ActiveSlotChanged += TransferItemData;
        _playerItemEquipper.ActiveSlotChanged += SubToItemUsed;

        TransferHealthData();
        TransferMaxHealthData();
    }

    public override void _Process(double delta)
    {
        TransferInteractData();
    }

    public override void _ExitTree()
    {
        _playerHealth.PlayerHealthChanged -= TransferHealthData;
        _playerItemEquipper.ActiveSlotChanged -= TransferItemData;
        _playerItemEquipper.ActiveSlotChanged -= SubToItemUsed;
    }

    private void TransferHealthData()
    {
        _HUDParameters.PlayerHealth = _playerHealth.CurrentHealth;
    }

    private void TransferMaxHealthData()
    {
        _HUDParameters.PlayerHealthMax = _playerHealth.MaxHealth;
    }

    private void TransferItemData(Item item)
    {
        _HUDParameters.PlayerActiveSlotItem = "";
        _HUDParameters.PlayerActiveSlotItem = item?.GetItemInfo();
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
        _HUDParameters.PlayerInteractData = "";

        if (!_rayOfLook.IsColliding())
            return;

        Node collider = _rayOfLook.GetCollider() as Node;
        if (collider.IsInGroup("Item:Pickupble") && collider is Item item)
            _HUDParameters.PlayerInteractData = $"[E] Pickup {item.Name}";
        else if (collider.IsInGroup("Hitbox:Player") && _playerItemEquipper.GetHoldingItem() is Medkit)
            _HUDParameters.PlayerInteractData = "[Hold right button] to heal teammate";
    }
}
