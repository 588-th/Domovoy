using Godot;
using System;

public partial class PlayerHotbar : Node
{
    [Export] private InputActions _inputActions;
    [Export] private AudioPlayer _audioPlayer;
    [Export] private Node playerNode;

    private HotbarSlot _activeSlot;
    private HotbarSlot _primarySlot;
    private HotbarSlot _secondarySlot;
    private HotbarSlot _tertiarySlot;

    public Action<HotbarSlot> ActiveSlotChanged;
    public Action<Item> ItemPlacedIntoActiveSlot;
    public Action<Item> ItemPlacedIntoInactiveSlot;
    public Action<Item> ItemAborted;

    public override void _Ready()
    {
        _inputActions.PrimarySlotDown += () => ActivateSlot(_primarySlot);
        _inputActions.SecondarySlotDown += () => ActivateSlot(_secondarySlot);
        _inputActions.TertiarySlotDown += () => ActivateSlot(_tertiarySlot);

        InitializeSlots();
        ActivateSlot(_primarySlot);
    }

    public override void _ExitTree()
    {
        _inputActions.PrimarySlotDown -= () => ActivateSlot(_primarySlot);
        _inputActions.SecondarySlotDown -= () => ActivateSlot(_secondarySlot);
        _inputActions.TertiarySlotDown -= () => ActivateSlot(_tertiarySlot);
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

        item.HoldingPlayer = playerNode;
        hotbarSlot.Item = item;
        if (_activeSlot == hotbarSlot)
            ItemPlacedIntoActiveSlot?.Invoke(item);
        else
        {
            _audioPlayer.PlayAudio(item.PickupAudio);
            ItemPlacedIntoInactiveSlot?.Invoke(item);
        }
    }

    public void AbortItem(Item item)
    {
        item.HoldingPlayer = null;
        GetSlot(item.SlotType).Item = null;
        ItemAborted?.Invoke(item);
    }

    private void InitializeSlots()
    {
        _primarySlot = new HotbarSlot(HotbarSlotType.Primary);
        _secondarySlot = new HotbarSlot(HotbarSlotType.Secondary);
        _tertiarySlot = new HotbarSlot(HotbarSlotType.Tertiary);
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
            default:
                throw new ArgumentException("Invalid hotbar slot type");
        }
    }
}
