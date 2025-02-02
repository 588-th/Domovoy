using Godot;
using System;

public partial class HUDParameters : Node
{
    private int _playerHealth;
    private int _playerHealthMax;
    private string _plaeyrName;
    private string _playerHoldingItem;
    private string _playerInteract;
    private string _playerAttack;

    public Action PlayerHealthUpdated;
    public Action PlayerHealthMaxUpdated;
    public Action PlayerNameUpdated;
    public Action PlayerHoldingItemUpdated;
    public Action PlayerInteractDataUpdated;
    public Action PlayerAttackDataUpdated;

    public int PlayerHealthMax
    {
        get { return _playerHealthMax; }
        set { _playerHealthMax = value; PlayerHealthMaxUpdated?.Invoke(); }
    }

    public int PlayerHealth
    {
        get {  return _playerHealth; }
        set { _playerHealth = value; PlayerHealthUpdated?.Invoke(); }
    }

    public string PlayerName
    {
        get { return _plaeyrName; }
        set { _plaeyrName = value; PlayerNameUpdated?.Invoke(); }
    }

    public string PlayerHoldingItem
    {
        get { return _playerHoldingItem; }
        set { _playerHoldingItem = value; PlayerHoldingItemUpdated?.Invoke(); }
    }

    public string PlayerInteract
    {
        get { return _playerInteract; }
        set { _playerInteract = value; PlayerInteractDataUpdated?.Invoke(); }
    }

    public string PlayerAttack
    {
        get { return _playerAttack; }
        set { _playerAttack = value; PlayerAttackDataUpdated?.Invoke(); }
    }
}
