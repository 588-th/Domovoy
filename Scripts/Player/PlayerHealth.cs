using Godot;
using System;

public partial class PlayerHealth : Node
{
    [Export] public int MaxHealth { get; private set; }
    [Export] public int CurrentHealth { get; private set; }

    public Action PlayerHealthZero;
    public Action PlayerHealthChanged;

    public void IncreaseHealth(int healthUnits)
    {
        if (CurrentHealth + healthUnits > MaxHealth)
            CurrentHealth = MaxHealth;
        else
            CurrentHealth += healthUnits;

        PlayerHealthChanged?.Invoke();
    }

    public void DecreaseHealth(int healthUnits)
    {
        if (CurrentHealth - healthUnits <= 0)
        {
            CurrentHealth = 0;
            PlayerHealthZero?.Invoke();
        }
        else
            CurrentHealth -= healthUnits;

        PlayerHealthChanged?.Invoke();
    }
}
