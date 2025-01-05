using Godot;
using System;

public partial class HUDParameters : Node
{
    private int _playerHealth;
    private int _playerHealthMax;
    private string _playerActiveSlotItem;
    private string _playerInteractData;

    public Action PlayerHealthUpdated;
    public Action PlayerHealthMaxUpdated;
    public Action PlayerActiveSlotItemUpdated;
    public Action PlayerInteractDataUpdated;

    [Export]
    public int PlayerHealthMax
    {
        get { return _playerHealthMax; }
        set { _playerHealthMax = value; PlayerHealthMaxUpdated?.Invoke(); }
    }

    [Export]
    public int PlayerHealth
    {
        get {  return _playerHealth; }
        set { _playerHealth = value; PlayerHealthUpdated?.Invoke(); }
    }

    [Export]
    public string PlayerActiveSlotItem
    {
        get { return _playerActiveSlotItem; }
        set { _playerActiveSlotItem = value; PlayerActiveSlotItemUpdated?.Invoke(); }
    }

    [Export]
    public string PlayerInteractData
    {
        get { return _playerInteractData; }
        set { _playerInteractData = value; PlayerInteractDataUpdated?.Invoke(); }
    }
}
