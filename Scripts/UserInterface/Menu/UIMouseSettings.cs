using Godot;

public partial class UIMouseSettings : Node
{
    [Export] private Control _settings;
    [Export] private SpinBox _sensivitySpinBox;
    [Export] private HSlider _sensivityHSlider;
    [Export] private Button _resetSeensivityButton;
    [Export] private Button _closeButton;

    public override void _Ready()
    {
        _closeButton.Pressed += () => _settings.Hide();

        InitializeSlider(_sensivityHSlider, _sensivitySpinBox, _resetSeensivityButton,
            () => SettingsMouse.Instance.Sensivity,
            value => SettingsMouse.Instance.Sensivity = value,
            SettingsMouse.Instance.DefaultSensivity);
    }

    private void InitializeSlider(HSlider slider, SpinBox spinBox, Button resetButton,
        System.Func<float> getSetting, System.Action<float> setSetting, float defaultSetting)
    {
        slider.Value = getSetting();
        spinBox.Value = slider.Value;
        resetButton.Disabled = getSetting() == defaultSetting;

        slider.ValueChanged += value =>
        {
            float floatValue = (float)value;
            setSetting(floatValue);
            spinBox.Value = floatValue;
            resetButton.Disabled = floatValue == defaultSetting;
        };

        spinBox.ValueChanged += value =>
        {
            float floatValue = (float)value;
            setSetting(floatValue);
            slider.Value = floatValue;
            resetButton.Disabled = floatValue == defaultSetting;
        };

        resetButton.Pressed += () =>
        {
            setSetting(defaultSetting);
            slider.Value = defaultSetting;
            spinBox.Value = defaultSetting;
        };
    }

    private void OnCloseButtonPressed()
    {
        _settings.Hide();
    }
}
