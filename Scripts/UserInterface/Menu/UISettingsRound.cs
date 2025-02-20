using Godot;

public partial class UISettingsRound : Node
{
    [Export] private Control _UIWindowSettings;
    [Export] private OptionButton _optionButtonMap;
    [Export] private Button _buttonClose;

    [Export] private SpinBox _spinBoxBattery;
    [Export] private HSlider _hSliderBattery;
    [Export] private Button _buttonResetBattery;

    [Export] private SpinBox _spinBoxHumanHealth;
    [Export] private HSlider _hSliderHumanHealth;
    [Export] private Button _buttonResetHumanHealth;

    [Export] private SpinBox _spinBoxMonsterHealth;
    [Export] private HSlider _hSliderMonsterHealth;
    [Export] private Button _buttonResetMonsterHealth;

    [Export] private SpinBox _spinBoxMonsterDamage;
    [Export] private HSlider _hSliderMonsterDamage;
    [Export] private Button _buttonResetMonsterDamage;

    private SpawnSystemLevel _spawnSystemLevel;

    private bool _isSubsctive;

    public override void _Ready()
    {
        if (MultiplayerConnection.Instance.ConnectionState != MultiplayerConnection.EnumConnectionState.Server)
            return;

        if (GameStage.Instance.CurrentGameStage != GameStage.EnumGameStage.Lobby)
            return;

        _optionButtonMap.ItemSelected += OnMapSelected;
        _buttonClose.Pressed += OnCloseButtonPressed;
        _isSubsctive = true;

        InitializeMap();

        InitializeSlider(_hSliderBattery, _spinBoxBattery, _buttonResetBattery,
            () => SettingsRound.Instance.HeadFlashlightBattery,
            value => SettingsRound.Instance.HeadFlashlightBattery = value,
            SettingsRound.Instance.DefaultHeadFlashlightBattery);

        InitializeSlider(_hSliderHumanHealth, _spinBoxHumanHealth, _buttonResetHumanHealth,
            () => SettingsRound.Instance.HumanHealth,
            value => SettingsRound.Instance.HumanHealth = value,
            SettingsRound.Instance.DefaultHumanHealth);

        InitializeSlider(_hSliderMonsterHealth, _spinBoxMonsterHealth, _buttonResetMonsterHealth,
            () => SettingsRound.Instance.MonsterHealth,
            value => SettingsRound.Instance.MonsterHealth = value,
            SettingsRound.Instance.DefaultMonsterHealth);

        InitializeSlider(_hSliderMonsterDamage, _spinBoxMonsterDamage, _buttonResetMonsterDamage,
            () => SettingsRound.Instance.MonsterDamage,
            value => SettingsRound.Instance.MonsterDamage = value,
            SettingsRound.Instance.DefaultMonsterDamage);
    }

    public override void _ExitTree()
    {
        if (!_isSubsctive)
            return;

        _optionButtonMap.ItemSelected -= OnMapSelected;
        _buttonClose.Pressed -= OnCloseButtonPressed;
    }

    private void InitializeMap()
    {
        Node node = GetTree().GetFirstNodeInGroup("SpawnSystem:Level");
        _spawnSystemLevel = node as SpawnSystemLevel;

        if (_spawnSystemLevel.RoundLevelScene.Count == 0)
            return;

        for (int i = 0; i < _spawnSystemLevel.RoundLevelScene.Count; i++)
            _optionButtonMap.AddItem(_spawnSystemLevel.RoundLevelScene[i].ResourcePath, i);

        _optionButtonMap.Selected = 0;
        SettingsRound.Instance.Map = _spawnSystemLevel.RoundLevelScene[0];
    }

    private void InitializeSlider(HSlider slider, SpinBox spinBox, Button resetButton,
    System.Func<int> getSetting, System.Action<int> setSetting, int defaultSetting)
    {
        setSetting(defaultSetting);
        slider.Value = getSetting();
        spinBox.Value = slider.Value;
        resetButton.Disabled = getSetting() == defaultSetting;

        slider.ValueChanged += value =>
        {
            int intValue = (int)value;
            setSetting(intValue);
            spinBox.Value = intValue;
            resetButton.Disabled = intValue == defaultSetting;
        };

        spinBox.ValueChanged += value =>
        {
            int intValue = (int)value;
            setSetting(intValue);
            slider.Value = intValue;
            resetButton.Disabled = intValue == defaultSetting;
        };

        resetButton.Pressed += () =>
        {
            setSetting(defaultSetting);
            slider.Value = defaultSetting;
            spinBox.Value = defaultSetting;
        };
    }

    private void OnMapSelected(long id)
    {
        if (id >= 0 && id < _spawnSystemLevel.RoundLevelScene.Count)
            SettingsRound.Instance.Map = _spawnSystemLevel.RoundLevelScene[(int)id];
    }

    private void OnCloseButtonPressed()
    {
        _UIWindowSettings.Hide();
    }
}
