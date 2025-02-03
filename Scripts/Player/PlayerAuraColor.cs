using Godot;

public partial class PlayerAuraColor : Node
{
    [Export] private PlayerHealth _playerHealth;
    [Export] private GpuParticles3D _aura;
    [Export] private int _maxAmount;
    [Export] private int _minAmount;

    private StandardMaterial3D _materialInstance;

    public override void _Ready()
    {
        _materialInstance = _aura.MaterialOverride.Duplicate() as StandardMaterial3D;
        _aura.MaterialOverride = _materialInstance;

        _playerHealth.PlayerHealthChanged += UpdateEffects;
    }

    public override void _ExitTree()
    {
        _playerHealth.PlayerHealthChanged -= UpdateEffects;
    }

    private void UpdateEffects()
    {
        float maxHealth = _playerHealth.MaxHealth;
        float currentHealth = _playerHealth.CurrentHealth;

        float healthPercent = Mathf.Clamp(currentHealth / maxHealth, 0f, 1f);

        float alpha = healthPercent;
        Color targetColor = new(1f, 1f, 1f, alpha);
        GlobalRpcFunctions.Instance.SetAlbedoOfGeometry(_aura.GetPath(), targetColor);

        int particleCount = Mathf.RoundToInt(Mathf.Lerp(_minAmount, _maxAmount, healthPercent));
        _aura.Amount = particleCount;
    }
}
