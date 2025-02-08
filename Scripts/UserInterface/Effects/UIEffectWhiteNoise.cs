using Godot;

public partial class UIEffectWhiteNoise : Node
{
    [Export] private HUDParameters _hudParameters;
    [Export] private ColorRect _colorRectWhiteNoise;

    private ShaderMaterial _shaderMaterial;

    public override void _Ready()
    {
        _shaderMaterial = _colorRectWhiteNoise.Material as ShaderMaterial;

        _hudParameters.PlayerHealthUpdated += UpdateWhiteNoiseEffect;
        _hudParameters.PlayerHealthMaxUpdated += UpdateWhiteNoiseEffect;
    }

    public override void _ExitTree()
    {
        _hudParameters.PlayerHealthUpdated -= UpdateWhiteNoiseEffect;
        _hudParameters.PlayerHealthMaxUpdated -= UpdateWhiteNoiseEffect;
    }

    private void UpdateWhiteNoiseEffect()
    {
        if (_hudParameters.PlayerHealth == 0 || _hudParameters.PlayerHealthMax == 0)
            return;

        float healthRatio = (float)_hudParameters.PlayerHealth / _hudParameters.PlayerHealthMax;
        float opasity = Mathf.Lerp(0.04f, 0f, healthRatio);
        _shaderMaterial.SetShaderParameter("noise_opacity", opasity);
    }
}
