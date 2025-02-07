using Godot;
using System;

public partial class PlayerHotbar : Node
{
    [Export] private Node _playerRoot;
    [Export] private InputActions _inputActions;
    [Export] private AudioPlayer _audioPlayer;

    private HotbarSlot _activeSlot;
    private HotbarSlot _primarySlot = new(HotbarSlotType.Primary);
    private HotbarSlot _secondarySlot = new(HotbarSlotType.Secondary);
    private HotbarSlot _tertiarySlot = new(HotbarSlotType.Tertiary);
    private HotbarSlot _quaternarySlot = new(HotbarSlotType.Quaternary);

    public Action<HotbarSlot> ActiveSlotChanged;
    public Action<Item> ItemPlacedIntoActiveSlot;
    public Action<Item> ItemPlacedIntoInactiveSlot;
    public Action<Item> ItemAborted;
    public Action<Item> ItemFree;

    public override void _Ready()
    {
        _inputActions.PrimarySlotDown += () => ActivateSlot(_primarySlot);
        _inputActions.SecondarySlotDown += () => ActivateSlot(_secondarySlot);
        _inputActions.TertiarySlotDown += () => ActivateSlot(_tertiarySlot);
        _inputActions.QuaternarySlotDown += () => ActivateSlot(_quaternarySlot);

        ActivateSlot(_primarySlot);
    }

    public override void _ExitTree()
    {
        _inputActions.PrimarySlotDown -= () => ActivateSlot(_primarySlot);
        _inputActions.SecondarySlotDown -= () => ActivateSlot(_secondarySlot);
        _inputActions.TertiarySlotDown -= () => ActivateSlot(_tertiarySlot);
        _inputActions.TertiarySlotDown -= () => ActivateSlot(_tertiarySlot);
        _inputActions.QuaternarySlotDown -= () => ActivateSlot(_quaternarySlot);
    }

    public bool IsHotbarSlotEmpty(HotbarSlotType slot)
    {
        return GetSlot(slot).IsEmpty();
    }

    public bool IsActiveSlotEmpty()
    {
        return _activeSlot.IsEmpty();
    }

    public Item GetActiveItem()
    {
        return _activeSlot.Item;
    }

    public Item GetItemFromSlot(HotbarSlotType slot)
    {
        return GetSlot(slot).Item;
    }

    public void PlaceItem(Item item)
    {
        ref HotbarSlot hotbarSlot = ref GetSlot(item.SlotType);
        if (!IsHotbarSlotEmpty(hotbarSlot.SlotType))
            return;

        item.HoldingPlayer = _playerRoot;
        hotbarSlot.Item = item;
        if (_activeSlot == hotbarSlot)
            ItemPlacedIntoActiveSlot?.Invoke(item);
        else
        {
            _audioPlayer.PlayAudio(item.PickupAudio);
            ItemPlacedIntoInactiveSlot?.Invoke(item);
        }

        item.ItemFree += OnItemFree;
    }

    public void AbortItem(Item item)
    {
        item.HoldingPlayer = null;
        GetSlot(item.SlotType).Item = null;
        ItemAborted?.Invoke(item);
    }

    private void OnItemFree(Item item)
    {
        item.ItemFree -= OnItemFree;
        item.HoldingPlayer = null;
        GetSlot(item.SlotType).Item = null;
        ItemFree?.Invoke(item);
    }

    private void ActivateSlot(HotbarSlot hotbarSlot)
    {
        if (_activeSlot == hotbarSlot)
            return;

        _activeSlot = hotbarSlot;
        ActiveSlotChanged?.Invoke(hotbarSlot);
    }

    private ref HotbarSlot GetSlot(HotbarSlotType slotType)
    {
        switch (slotType)
        {
            case HotbarSlotType.Primary:
                return ref _primarySlot;
            case HotbarSlotType.Secondary:
                return ref _secondarySlot;
            case HotbarSlotType.Tertiary:
                return ref _tertiarySlot;
            case HotbarSlotType.Quaternary:
                return ref _quaternarySlot;
            default:
                throw new ArgumentException("Invalid hotbar slot type");
        }
    }
}
