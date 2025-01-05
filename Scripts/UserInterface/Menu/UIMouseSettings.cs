using Godot;
using System;

public partial class UIMouseSettings : Node
{
    [Export] private Control _menu;
    [Export] private Control _settings;
    [Export] private SpinBox _sensivitySpinBox;
    [Export] private HSlider _sensivityHSlider;
    [Export] private Button _backButton;

    public override void _Ready()
    {
        _backButton.Pressed += OnBackButtonPressed;
        _sensivitySpinBox.ValueChanged += OnSensivitySpinBoxChanged;
        _sensivityHSlider.ValueChanged += OnSensivityHSliderChanged;
        InitializeUI();
    }

    public override void _ExitTree()
    {
        _backButton.Pressed -= OnBackButtonPressed;
        _sensivitySpinBox.ValueChanged -= OnSensivitySpinBoxChanged;
        _sensivityHSlider.ValueChanged -= OnSensivityHSliderChanged;
    }

    private void InitializeUI()
    {
        _sensivitySpinBox.Value = SettingsMouse.Instance.Sensivity;
        _sensivityHSlider.Value = SettingsMouse.Instance.Sensivity;
    }

    private void OnSensivitySpinBoxChanged(double value)
    {
        _sensivityHSlider.Value = value;
        SettingsMouse.Instance.Sensivity = (float)value;
    }

    private void OnSensivityHSliderChanged(double value)
    {
        _sensivitySpinBox.Value = value;
        SettingsMouse.Instance.Sensivity = (float)value;
    }

    private void OnBackButtonPressed()
    {
        _menu.Show();
        _settings.Hide();
    }
}
