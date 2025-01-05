using Godot;
using System.Collections.Generic;
using static Godot.DisplayServer;

public partial class UISettingsVideo : Control
{
    [Export] private Control _menu;
    [Export] private Control _settings;
    [Export] private OptionButton _resolutionsOptionButton;
    [Export] private OptionButton _windowModeOptionButton;
    [Export] private SpinBox _fovSpinBox;
    [Export] private HSlider _fovHSlider;
    [Export] private Button _backButton;

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
        _backButton.Pressed += OnBackButtonPressed;
        _resolutionsOptionButton.ItemSelected += OnResolutionSelected;
        _windowModeOptionButton.ItemSelected += OnWindowModeSelected;
        _fovSpinBox.ValueChanged += OnFovSpinBoxChanged;
        _fovHSlider.ValueChanged += OnFovHSliderBoxChanged;

        InitializeUI();
    }

    public override void _ExitTree()
    {
        _backButton.Pressed -= OnBackButtonPressed;
        _resolutionsOptionButton.ItemSelected -= OnResolutionSelected;
        _windowModeOptionButton.ItemSelected -= OnWindowModeSelected;
    }

    private void InitializeUI()
    {
        LoadFOV();
        LoadResolution();
        LoadWindowMode();
    }

    private void LoadFOV()
    {
        _fovSpinBox.Value = SettingsVideo.Instance.FOV;
        _fovHSlider.Value = SettingsVideo.Instance.FOV;
    }

    private void LoadResolution()
    {
        _resolutionsOptionButton.Selected = -1;
        Vector2I currentResolution = GetWindow().Size;

        int resolutionIndex = 0;
        foreach (var resolution in _resolutions.Keys)
        {
            _resolutionsOptionButton.AddItem(resolution);
            if (_resolutions[resolution] == currentResolution)
            {
                _resolutionsOptionButton.Selected = resolutionIndex;
            }
            resolutionIndex++;
        }
    }

    private void LoadWindowMode()
    {
        _windowModeOptionButton.Selected = -1;
        int currentMode = (int)GetWindow().Mode;

        int modeIndex = 0;
        foreach (var windowMode in _windowModes)
        {
            _windowModeOptionButton.AddItem(windowMode.ToString());
            if ((int)windowMode == currentMode)
            {
                _windowModeOptionButton.Selected = modeIndex;
            }
            modeIndex++;
        }
    }

    private void OnResolutionSelected(long id)
    {
        string selectedResolution = _resolutionsOptionButton.GetItemText((int)id);
        WindowSetSize(_resolutions[selectedResolution]);

        Vector2I screenCenter = ScreenGetPosition() + ScreenGetSize() / 2;
        Vector2I windowSize = GetWindow().GetSizeWithDecorations();
        GetWindow().Position = screenCenter - windowSize / 2;
    }

    private void OnWindowModeSelected(long id)
    {
        WindowMode selectedMode = _windowModes[(int)id];
        WindowSetMode(selectedMode);
    }

    public void OnFovSpinBoxChanged(double value)
    {
        _fovHSlider.Value = value;
        SettingsVideo.Instance.FOV = (float)value;
    }

    public void OnFovHSliderBoxChanged(double value)
    {
        _fovSpinBox.Value = value;
        SettingsVideo.Instance.FOV = (float)value;
    }

    private void OnBackButtonPressed()
    {
        _menu.Show();
        _settings.Hide();
    }
}
