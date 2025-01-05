using Godot;
using System;

public partial class PlayerAuraColor : Node
{
    [Export] private PlayerHealth _playerHealth;
    [Export] private GeometryInstance3D _geometry;

    private Material _materialInstance;

    public override void _Ready()
    {
        if (_geometry != null && _geometry.MaterialOverride != null)
        {
            _materialInstance = _geometry.MaterialOverride.Duplicate() as Material;
            _geometry.MaterialOverride = _materialInstance;
        }

        if (_playerHealth != null)
        {
            _playerHealth.PlayerHealthChanged += UpdateColor;
            UpdateColor();
        }
    }

    public override void _ExitTree()
    {
        if (_playerHealth != null)
        {
            _playerHealth.PlayerHealthChanged -= UpdateColor;
        }
    }

    private void UpdateColor()
    {
        if (_playerHealth == null || _materialInstance == null)
            return;

        float maxHealth = _playerHealth.MaxHealth;
        float currentHealth = _playerHealth.CurrentHealth;

        float healthPercent = Mathf.Clamp(currentHealth / maxHealth, 0f, 1f);

        Color targetColor = new Color(1f - healthPercent, healthPercent, 0f);

        if (_materialInstance is StandardMaterial3D standardMaterial)
        {
            standardMaterial.AlbedoColor = targetColor;
        }
    }
}
