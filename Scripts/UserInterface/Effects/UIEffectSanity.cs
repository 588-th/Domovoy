using Godot;

public partial class UIEffectSanity : Node
{
    [Export] private HUDParameters _hudParameters;
    [Export] private ColorRect _colorRectSanity;

    private ShaderMaterial _shaderMaterial;

    public override void _Ready()
    {
        _shaderMaterial = _colorRectSanity.Material as ShaderMaterial;

        _hudParameters.PlayerHealthUpdated += UpdateSanityEffect;
        _hudParameters.PlayerHealthMaxUpdated += UpdateSanityEffect;
    }

    public override void _ExitTree()
    {
        _hudParameters.PlayerHealthUpdated -= UpdateSanityEffect;
        _hudParameters.PlayerHealthMaxUpdated -= UpdateSanityEffect;
    }

    private void UpdateSanityEffect()
    {
        if (_hudParameters.PlayerHealth == 0 || _hudParameters.PlayerHealthMax == 0)
            return;

        float healthRatio = (float)_hudParameters.PlayerHealth / _hudParameters.PlayerHealthMax;
        float saturation = Mathf.Lerp(0.0f, 1.0f, healthRatio);
        _shaderMaterial.SetShaderParameter("saturation", saturation);
    }
}
