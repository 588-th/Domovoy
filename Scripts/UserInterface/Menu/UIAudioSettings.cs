using Godot;

public partial class UIAudioSettings : Node
{
    [Export] private Control _settings;
    [Export] private Button _closeButton;

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

    private const string MasterBus = "Master";
    private const string InterfaceBus = "Interface";
    private const string EffectsBus = "SFX";
    private const string VoiceChatBus = "VoiceChat";

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

    private void ApplyVolume(string busName, int volume)
    {
        int busIndex = AudioServer.GetBusIndex(busName);
        if (busIndex != -1)
            AudioServer.SetBusVolumeDb(busIndex, volume);
    }
}
