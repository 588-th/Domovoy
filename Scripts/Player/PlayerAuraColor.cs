using Godot;

public partial class PlayerAuraColor : Node
{
    [Export] private PlayerHealth _playerHealth;
    [Export] private GeometryInstance3D _geometry;

    private StandardMaterial3D _materialInstance;

    public override void _Ready()
    {
        _materialInstance = _geometry.MaterialOverride.Duplicate() as StandardMaterial3D;
        _geometry.MaterialOverride = _materialInstance;

        _playerHealth.PlayerHealthChanged += UpdateColor;
    }

    public override void _ExitTree()
    {
        _playerHealth.PlayerHealthChanged -= UpdateColor;
    }

    private void UpdateColor()
    {
        float maxHealth = _playerHealth.MaxHealth;
        float currentHealth = _playerHealth.CurrentHealth;

        float healthPercent = Mathf.Clamp(currentHealth / maxHealth, 0f, 1f);

        Color baseColor = new(1f - healthPercent, healthPercent, 0f);

        float dimFactor = 1f;
        Color targetColor = baseColor * dimFactor;

        RpcFunctions.Instance.ChangeAlbedoOfGeometry(_geometry.GetPath(), targetColor);
    }
}
