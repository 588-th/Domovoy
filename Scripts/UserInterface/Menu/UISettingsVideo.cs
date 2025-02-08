using Godot;
using System.Collections.Generic;
using static Godot.DisplayServer;

public partial class UISettingsVideo : Control
{
    [Export] private Control _UIWindowSettings;
    [Export] private OptionButton _optionButtonWindowMode;
    [Export] private OptionButton _optionButtonResolutions;
    [Export] private SpinBox _spinBoxFOV;
    [Export] private HSlider _hSliderFOV;
    [Export] private Button _buttonClose;

    private readonly Dictionary<string, Vector2I> _resolutions = new()
    {
        { "800x600", new Vector2I(800, 600)},
        { "1024x768", new Vector2I(1024, 768)},
        { "1152x648", new Vector2I(1152, 648)},
        { "1280x720", new Vector2I(1280, 720)},
        { "1366x768", new Vector2I(1366, 768)},
        { "1440x900", new Vector2I(1440, 900)},
        { "1600x900", new Vector2I(1600, 900)},
        { "1680x1050", new Vector2I(1680, 1050)},
        { "1920x1080", new Vector2I(1920, 1080)},
        { "2560x1440", new Vector2I(2560, 1440)},
        { "3840x2160", new Vector2I(3840, 2160)}
    };

    private readonly List<WindowMode> _windowModes = new()
    {
        WindowMode.Fullscreen,
        WindowMode.Windowed
    };

    public override void _Ready()
    {
        _optionButtonWindowMode.ItemSelected += OnWindowModeSelected;
        _optionButtonResolutions.ItemSelected += OnResolutionSelected;
        _spinBoxFOV.ValueChanged += OnFovSpinBoxChanged;
        _hSliderFOV.ValueChanged += OnFovHSliderBoxChanged;
        _buttonClose.Pressed += OnCloseButtonPressed;

        InitializeUI();
    }

    public override void _ExitTree()
    {
        _optionButtonWindowMode.ItemSelected -= OnWindowModeSelected;
        _optionButtonResolutions.ItemSelected -= OnResolutionSelected;
        _spinBoxFOV.ValueChanged -= OnFovSpinBoxChanged;
        _hSliderFOV.ValueChanged -= OnFovHSliderBoxChanged;
        _buttonClose.Pressed -= OnCloseButtonPressed;
    }

    private void InitializeUI()
    {
        LoadWindowMode();
        LoadResolution();
        LoadFOV();
    }

    private void LoadWindowMode()
    {
        _optionButtonWindowMode.Selected = -1;
        int currentMode = (int)GetWindow().Mode;

        int modeIndex = 0;
        foreach (var windowMode in _windowModes)
        {
            _optionButtonWindowMode.AddItem(windowMode.ToString());
            if ((int)windowMode == currentMode)
            {
                _optionButtonWindowMode.Selected = modeIndex;
            }
            modeIndex++;
        }
    }

    private void LoadResolution()
    {
        _optionButtonResolutions.Selected = -1;
        Vector2I currentResolution = GetWindow().Size;

        int resolutionIndex = 0;
        foreach (var resolution in _resolutions.Keys)
        {
            _optionButtonResolutions.AddItem(resolution);
            if (_resolutions[resolution] == currentResolution)
            {
                _optionButtonResolutions.Selected = resolutionIndex;
            }
            resolutionIndex++;
        }
    }

    private void LoadFOV()
    {
        _spinBoxFOV.Value = SettingsVideo.Instance.FOV;
        _hSliderFOV.Value = SettingsVideo.Instance.FOV;
    }

    private void OnWindowModeSelected(long id)
    {
        WindowMode selectedMode = _windowModes[(int)id];
        WindowSetMode(selectedMode);
    }

    private void OnResolutionSelected(long id)
    {
        string selectedResolution = _optionButtonResolutions.GetItemText((int)id);
        WindowSetSize(_resolutions[selectedResolution]);

        Vector2I screenCenter = ScreenGetPosition() + ScreenGetSize() / 2;
        Vector2I windowSize = GetWindow().GetSizeWithDecorations();
        GetWindow().Position = screenCenter - windowSize / 2;
    }

    public void OnFovSpinBoxChanged(double value)
    {
        _hSliderFOV.Value = value;
        SettingsVideo.Instance.FOV = (float)value;
    }

    public void OnFovHSliderBoxChanged(double value)
    {
        _spinBoxFOV.Value = value;
        SettingsVideo.Instance.FOV = (float)value;
    }

    private void OnCloseButtonPressed()
    {
        _UIWindowSettings.Hide();
    }
}
