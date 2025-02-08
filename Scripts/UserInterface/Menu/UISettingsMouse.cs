using Godot;

public partial class UISettingsMouse : Node
{
    [Export] private Control _UIWindowSettings;
    [Export] private SpinBox _spinBoxSensivity;
    [Export] private HSlider _hSliderSensivity;
    [Export] private Button _buttonResetSensivity;
    [Export] private Button _buttonClose;

    public override void _Ready()
    {
        _buttonClose.Pressed += () => _UIWindowSettings.Hide();

        InitializeSlider(_hSliderSensivity, _spinBoxSensivity, _buttonResetSensivity,
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
        _UIWindowSettings.Hide();
    }
}
