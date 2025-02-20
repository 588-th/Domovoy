using Godot;
using System;

public partial class PlayerHealth : Node
{
    [ExportGroup("Parameters")]
    [Export] private int _maxHealth = 100;
    [Export] private int _currentHealth = 100;

    public Action PlayerHealthZero;
    public Action PlayerHealthChanged;
    public Action PlayerHealthIncrease;
    public Action PlayerHealthDecrease;

    public void SetMaxHealth(int value)
    {
        _maxHealth = value;
    }

    public void SetCurrentHealth(int value)
    {
        if (value > _maxHealth)
            return;

        _currentHealth = value;
        PlayerHealthChanged?.Invoke();
    }

    public int GetMaxHealth()
    {
        return _maxHealth;
    }

    public int GetCurrentHealth()
    {
        return _currentHealth;
    }

    public void IncreaseHealth(int healthUnits)
    {
        if (_currentHealth + healthUnits > _maxHealth)
            _currentHealth = _maxHealth;
        else
            _currentHealth += healthUnits;

        PlayerHealthChanged?.Invoke();
        PlayerHealthIncrease?.Invoke();
    }

    public void DecreaseHealth(int healthUnits)
    {
        if (_currentHealth - healthUnits <= 0)
        {
            _currentHealth = 0;
            PlayerHealthZero?.Invoke();
        }
        else
            _currentHealth -= healthUnits;

        PlayerHealthChanged?.Invoke();
        PlayerHealthDecrease?.Invoke();
    }
}
