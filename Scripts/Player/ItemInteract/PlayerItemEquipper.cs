using Godot;
using System;
using System.Collections.Generic;

public partial class PlayerItemEquipper : Node
{
    [Export] private InputActions _inputActions;
    [Export] private AudioPlayer _audioPlayer;
    [Export] private PlayerHotbar _playerHotbar;
    [Export] private Marker3D _anchorHandItem;
    [Export] private Marker3D _anchorPrimatyItem;
    [Export] private Marker3D _anchorSecondaryItem;
    [Export] private Marker3D _anchorTertiaryItem;
    [Export] private Marker3D _anchorQuaternaryItem;

    private readonly List<Item> _equipedItems = new();
    private Item _holdingItem;

    /// <summary>
    /// Item can be null
    /// </summary>
    public Action<Item> ActiveSlotChanged;
    public Action<Item> ItemOnActiveSlotChanged;

    public override void _Ready()
    {
        _playerHotbar.ItemPlacedIntoActiveSlot += TakeItemOnHand;
        _playerHotbar.ItemPlacedIntoInactiveSlot += EquipItem;
        _playerHotbar.ActiveSlotChanged += OnActiveSlotChanged;
        _playerHotbar.ItemAborted += OnItemAborted;
        _playerHotbar.ItemFree += OnItemFree;
    }

    public override void _ExitTree()
    {
        _playerHotbar.ItemPlacedIntoActiveSlot -= TakeItemOnHand;
        _playerHotbar.ItemPlacedIntoInactiveSlot -= EquipItem;
        _playerHotbar.ActiveSlotChanged -= OnActiveSlotChanged;
        _playerHotbar.ItemAborted -= OnItemAborted;
        _playerHotbar.ItemFree -= OnItemFree;
    }

    public Item GetHoldingItem()
    {
        return _holdingItem;
    }

    private void OnActiveSlotChanged(HotbarSlot hotbarSlot)
    {
        if (_holdingItem != null)
            EquipItem(_holdingItem);

        _holdingItem = null;

        if (!hotbarSlot.IsEmpty())
            TakeItemOnHand(hotbarSlot.Item);

        ActiveSlotChanged?.Invoke(hotbarSlot.Item);
    }

    private void OnItemFree(Item item)
    {
        if (_holdingItem == item)
        {
            item.UnbindOnHandActions(_inputActions);
            ItemOnActiveSlotChanged?.Invoke(item);
            _holdingItem = null;
        }
        else
        {
            item.UnbindEquipActions(_inputActions);
            _equipedItems.Remove(item);
        }
    }

    private void OnItemAborted(Item item)
    {
        if (_holdingItem == item)
        {
            item.UnbindOnHandActions(_inputActions);
            ItemOnActiveSlotChanged?.Invoke(item);
            _holdingItem = null;
        }
        else
        {
            item.UnbindEquipActions(_inputActions);
            _equipedItems.Remove(item);
        }

        Node holderItems = GetTree().GetFirstNodeInGroup("Holder:Items");
        item.GetParent().RemoveChild(item);
        holderItems.AddChild(item, true);

        item.EmitSignal(SignalName.Ready);
    }

    private void EquipItem(Item item)
    {
        Marker3D achorSlot = GetAchorSlot(item.SlotType);

        item.GetParent().RemoveChild(item);
        achorSlot.AddChild(item, true);
        item.EmitSignal(SignalName.Ready);

        item.BindEquipActions(_inputActions);
        item.UnbindOnHandActions(_inputActions);
        item.Position = Vector3.Zero;
        item.Rotation = Vector3.Zero;

        _equipedItems.Add(item);
    }

    private void TakeItemOnHand(Item item)
    {
        item.GetParent().RemoveChild(item);
        _anchorHandItem.AddChild(item, true);
        item.EmitSignal(SignalName.Ready);

        item.BindOnHandActions(_inputActions);
        item.UnbindEquipActions(_inputActions);
        item.Position = Vector3.Zero;
        item.Rotation = Vector3.Zero;
        _holdingItem = item;

        _audioPlayer.PlayAudio(item.TakeOnHandAudio);

        ActiveSlotChanged?.Invoke(item);
    }

    private ref Marker3D GetAchorSlot(HotbarSlotType slot)
    {
        switch (slot)
        {
            case HotbarSlotType.Primary:
                return ref _anchorPrimatyItem;
            case HotbarSlotType.Secondary:
                return ref _anchorSecondaryItem;
            case HotbarSlotType.Tertiary:
                return ref _anchorTertiaryItem;
            case HotbarSlotType.Quaternary:
                return ref _anchorQuaternaryItem;
            default:
                throw new ArgumentException("Invalid slot name");
        }
    }
}
