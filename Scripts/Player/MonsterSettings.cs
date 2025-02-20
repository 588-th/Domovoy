using Godot;
using System;

public partial class MonsterSettings : Node
{
    [Export] private PlayerHealth _playerHealth;
    [Export] private PlayerMeleeAttack _playerMeleeAttack;

    public override void _Ready()
    {
        SetSettings();
    }

    private void SetSettings()
    {
        _playerHealth.SetMaxHealth(SettingsRound.Instance.MonsterHealth);
        _playerHealth.SetCurrentHealth(SettingsRound.Instance.MonsterHealth);

        _playerMeleeAttack.SetAttackDamage(SettingsRound.Instance.MonsterDamage);
    }
}
