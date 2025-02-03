using Godot;

public partial class UIAudioSettings : Node
{
    [Export] private Control _menu;
    [Export] private Control _settings;
    [Export] private HSlider _masterHSlider;
    [Export] private HSlider _interfaceHSlider;
    [Export] private HSlider _effectsHSlider;
    [Export] private HSlider _voiceChatHSlider;
    [Export] private Button _backButton;

    private const string MasterBusName = "Master";
    private const string InterfaceBusName = "Interface";
    private const string EffectsBusName = "SFX";
    private const string VoiceChatBusName = "VoiceChat";

    public override void _Ready()
    {
        _backButton.Pressed += OnBackButtonPressed;
        _masterHSlider.ValueChanged += OnMasterHSliderChanged;
        _interfaceHSlider.ValueChanged += OnInterfaceHSliderChanged;
        _effectsHSlider.ValueChanged += OnEffectsHSliderChanged;
        _voiceChatHSlider.ValueChanged += OnVoiceChatHSliderChanged;

        InitializeUI();
    }

    public override void _ExitTree()
    {
        _backButton.Pressed -= OnBackButtonPressed;
        _masterHSlider.ValueChanged -= OnMasterHSliderChanged;
        _interfaceHSlider.ValueChanged -= OnInterfaceHSliderChanged;
        _effectsHSlider.ValueChanged -= OnEffectsHSliderChanged;
        _voiceChatHSlider.ValueChanged -= OnVoiceChatHSliderChanged;
    }

    private void InitializeUI()
    {
        _masterHSlider.Value = SettingsAudio.Instance.Master;
        _interfaceHSlider.Value = SettingsAudio.Instance.Interface;
        _effectsHSlider.Value = SettingsAudio.Instance.Effects;
        _voiceChatHSlider.Value = SettingsAudio.Instance.VoiceChat;

        ApplyVolume(MasterBusName, SettingsAudio.Instance.Master);
        ApplyVolume(InterfaceBusName, SettingsAudio.Instance.Interface);
        ApplyVolume(EffectsBusName, SettingsAudio.Instance.Effects);
        ApplyVolume(VoiceChatBusName, SettingsAudio.Instance.VoiceChat);
    }

    private void OnMasterHSliderChanged(double value)
    {
        SettingsAudio.Instance.Master = (int)value;
        ApplyVolume(MasterBusName, (int)value);
    }

    private void OnInterfaceHSliderChanged(double value)
    {
        SettingsAudio.Instance.Interface = (int)value;
        ApplyVolume(InterfaceBusName, (int)value);
    }

    private void OnEffectsHSliderChanged(double value)
    {
        SettingsAudio.Instance.Effects = (int)value;
        ApplyVolume(EffectsBusName, (int)value);
    }

    private void OnVoiceChatHSliderChanged(double value)
    {
        SettingsAudio.Instance.VoiceChat = (int)value;
        ApplyVolume(VoiceChatBusName, (int)value);
    }

    private void ApplyVolume(string busName, int volume)
    {
        int busIndex = AudioServer.GetBusIndex(busName);
        if (busIndex == -1)
            return;

        AudioServer.SetBusVolumeDb(busIndex, volume);
    }

    private void OnBackButtonPressed()
    {
        _menu.Show();
        _settings.Hide();
    }
}
