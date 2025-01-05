using Godot;

public partial class PlayerHUD : Node
{
    [Export] private HUDParameters _HUDParameters;
    
    [Export] private Label _UIplayerHealth;
    [Export] private Label _UIitem;
    [Export] private Label _UIinteractInfo;
    [Export] private ColorRect _UISanity;

    private ShaderMaterial _shaderMaterial;

    public override void _Ready()
    {
        _shaderMaterial = _UISanity.Material as ShaderMaterial;

        _HUDParameters.PlayerHealthUpdated += UpdateUIHealth;
        _HUDParameters.PlayerHealthUpdated += UpdateSaturation;
        _HUDParameters.PlayerHealthMaxUpdated += UpdateSaturation;
        _HUDParameters.PlayerActiveSlotItemUpdated += UpdateUIItem;
        _HUDParameters.PlayerInteractDataUpdated += UpdateUIInteractInfo;

        UpdateUIHealth();
    }

    public override void _ExitTree()
    {
        _HUDParameters.PlayerHealthUpdated -= UpdateUIHealth;
        _HUDParameters.PlayerHealthUpdated -= UpdateSaturation;
        _HUDParameters.PlayerHealthMaxUpdated -= UpdateSaturation;
        _HUDParameters.PlayerActiveSlotItemUpdated -= UpdateUIItem;
        _HUDParameters.PlayerInteractDataUpdated -= UpdateUIInteractInfo;
    }

    private void UpdateUIHealth()
    {
        _UIplayerHealth.Text = _HUDParameters.PlayerHealth.ToString();
    }

    private void UpdateUIItem()
    {
        _UIitem.Text = _HUDParameters.PlayerActiveSlotItem;
    }

    private void UpdateUIInteractInfo()
    {
        _UIinteractInfo.Text = _HUDParameters.PlayerInteractData;
    }

    private void UpdateSaturation()
    {
        if (_HUDParameters.PlayerHealth == 0 || _HUDParameters.PlayerHealthMax == 0)
            return;

        float healthRatio = (float)_HUDParameters.PlayerHealth / _HUDParameters.PlayerHealthMax;
        float saturation = Mathf.Lerp(0.0f, 1.0f, healthRatio);
        _shaderMaterial.SetShaderParameter("saturation", saturation);
    }
}
