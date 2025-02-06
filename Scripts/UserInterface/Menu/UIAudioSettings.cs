using Godot;

public partial class UIAudioSettings : Node
{
    [Export] private Control _settings;
    [Export] private Button _closeButton;
    [Export] private CheckBox _muteYourselfCheckBox;

    [Export] private HSlider _masterHSlider;
    [Export] private HSlider _interfaceHSlider;
    [Export] private HSlider _effectsHSlider;
    [Export] private HSlider _voiceChatHSlider;

    [Export] private SpinBox _masterSpinBox;
    [Export] private SpinBox _interfaceSpinBox;
    [Export] private SpinBox _effectsSpinBox;
    [Export] private SpinBox _voiceChatSpinBox;

    [Export] private Button _resetMasterButton;
    [Export] private Button _resetInterfaceButton;
    [Export] private Button _resetEffectsButton;
    [Export] private Button _resetVoiceChatButton;
    [Export] private Button _resetMuteYourselfButton;

    private const string MasterBus = "Master";
    private const string InterfaceBus = "Interface";
    private const string EffectsBus = "SFX";
    private const string VoiceChatBus = "VoiceChat";
    private const string VoiceSelfMute = "VoiceSelfMute";

    public override void _Ready()
    {
        _closeButton.Pressed += () => _settings.Hide();

        InitializeSlider(_masterHSlider, _masterSpinBox, _resetMasterButton, MasterBus,
            () => SettingsAudio.Instance.Master,
            value => SettingsAudio.Instance.Master = value,
            SettingsAudio.Instance.DefaultMaster);

        InitializeSlider(_interfaceHSlider, _interfaceSpinBox, _resetInterfaceButton, InterfaceBus,
            () => SettingsAudio.Instance.Interface,
            value => SettingsAudio.Instance.Interface = value,
            SettingsAudio.Instance.DefaultInterface);

        InitializeSlider(_effectsHSlider, _effectsSpinBox, _resetEffectsButton, EffectsBus,
            () => SettingsAudio.Instance.Effects,
            value => SettingsAudio.Instance.Effects = value,
            SettingsAudio.Instance.DefaultEffects);

        InitializeSlider(_voiceChatHSlider, _voiceChatSpinBox, _resetVoiceChatButton, VoiceChatBus,
            () => SettingsAudio.Instance.VoiceChat,
            value => SettingsAudio.Instance.VoiceChat = value,
            SettingsAudio.Instance.DefaultVoiceChat);

        InitializeCheckBox(_muteYourselfCheckBox, _resetMuteYourselfButton, VoiceSelfMute,
            () => SettingsAudio.Instance.MuteYourself,
            value => SettingsAudio.Instance.MuteYourself = value,
            SettingsAudio.Instance.DefaultMuteYourself);
    }

    private void InitializeSlider(HSlider slider, SpinBox spinBox, Button resetButton, string busName,
        System.Func<int> getSetting, System.Action<int> setSetting, int defaultSetting)
    {
        slider.Value = getSetting();
        spinBox.Value = slider.Value;
        resetButton.Disabled = getSetting() == defaultSetting;

        slider.ValueChanged += value => 
        {
            int intValue = (int)value;
            setSetting(intValue);
            spinBox.Value = intValue;
            resetButton.Disabled = intValue == defaultSetting;
            ApplyVolume(busName, intValue);
        };

        spinBox.ValueChanged += value =>
        {
            int intValue = (int)value;
            setSetting(intValue);
            slider.Value = intValue;
            resetButton.Disabled = intValue == defaultSetting;
            ApplyVolume(busName, intValue);
        };

        resetButton.Pressed += () => 
        {
            setSetting(defaultSetting);
            slider.Value = defaultSetting;
            spinBox.Value = defaultSetting;
        };
    }

    private void InitializeCheckBox(CheckBox checkBox, Button resetButton, string busName,
        System.Func<bool> getSetting, System.Action<bool> setSetting, bool defaultSetting)
    {
        checkBox.ButtonPressed = getSetting();

        checkBox.Pressed += () =>
        {
            MuteBus(busName, checkBox.ButtonPressed);
        };

        resetButton.Pressed += () =>
        {
            setSetting(defaultSetting);
            checkBox.ButtonPressed = defaultSetting;
        };
    }

    private void ApplyVolume(string busName, int volume)
    {
        int busIndex = AudioServer.GetBusIndex(busName);
        if (busIndex != -1)
            AudioServer.SetBusVolumeDb(busIndex, volume);
    }

    private void MuteBus(string busName, bool mute)
    {
        int busIndex = AudioServer.GetBusIndex(busName);
        if (busIndex != -1)
            AudioServer.SetBusMute(busIndex, mute);
    }
}
