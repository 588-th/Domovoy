using Godot;

public partial class UISettingsAudio : Node
{
    [Export] private Control _UIWindowSettings;

    [Export] private HSlider _hSliderMaster;
    [Export] private HSlider _hSliderInterface;
    [Export] private HSlider _hSliderEffects;
    [Export] private HSlider _hSliderVoiceChat;

    [Export] private SpinBox _spinBoxMaster;
    [Export] private SpinBox _spinBoxInterface;
    [Export] private SpinBox _spinBoxEffects;
    [Export] private SpinBox _spinBoxVoiceChat;

    [Export] private CheckBox _checkBoxMuteYourself;

    [Export] private Button _buttonResetMaster;
    [Export] private Button _buttonResetInterface;
    [Export] private Button _buttonResetEffects;
    [Export] private Button _buttonResetVoiceChat;
    [Export] private Button _buttonResetMuteYourself;
    [Export] private Button _buttonClose;

    private const string MasterBus = "Master";
    private const string InterfaceBus = "Interface";
    private const string EffectsBus = "Effects";
    private const string VoiceChatBus = "VoiceChat";
    private const string VoiceSelfMute = "VoiceSelfMute";

    public override void _Ready()
    {
        _buttonClose.Pressed += () => _UIWindowSettings.Hide();

        InitializeSlider(_hSliderMaster, _spinBoxMaster, _buttonResetMaster, MasterBus,
            () => SettingsAudio.Instance.Master,
            value => SettingsAudio.Instance.Master = value,
            SettingsAudio.Instance.DefaultMaster);

        InitializeSlider(_hSliderInterface, _spinBoxInterface, _buttonResetInterface, InterfaceBus,
            () => SettingsAudio.Instance.Interface,
            value => SettingsAudio.Instance.Interface = value,
            SettingsAudio.Instance.DefaultInterface);

        InitializeSlider(_hSliderEffects, _spinBoxEffects, _buttonResetEffects, EffectsBus,
            () => SettingsAudio.Instance.Effects,
            value => SettingsAudio.Instance.Effects = value,
            SettingsAudio.Instance.DefaultEffects);

        InitializeSlider(_hSliderVoiceChat, _spinBoxVoiceChat, _buttonResetVoiceChat, VoiceChatBus,
            () => SettingsAudio.Instance.VoiceChat,
            value => SettingsAudio.Instance.VoiceChat = value,
            SettingsAudio.Instance.DefaultVoiceChat);

        InitializeCheckBox(_checkBoxMuteYourself, _buttonResetMuteYourself, VoiceSelfMute,
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
