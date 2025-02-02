using Godot;

public partial class HeadFlashlightToggleLight : Node
{
    [Export] private HeadFlashlightBattery _headFlashlightBattery;
    [Export] private HeadFlashlightFlicker _headFlashlightFlicker;
    [Export] private SpotLight3D _flashlight;
    [Export] private float _lightEnergy;

    [ExportGroup("Audio")]
    [Export] private AudioPlayer3D _audioPlayer3D;
    [Export] private AudioStreamMP3 _flashlightOnAudio;
    [Export] private AudioStreamMP3 _flashlightOffAudio;
    [Export] private AudioStreamMP3 _flashlightOnDischargedAudio;

    public bool IsFlashlightEnabled { get; private set; }

    public override void _Process(double delta)
    {
        if (!IsFlashlightEnabled)
            return;

        if (_headFlashlightBattery.IsDepleted())
        {
            ToggleOffFlashlight();
            return;
        }
    }

    public void ToggleFlashlight()
    {
        if (IsFlashlightEnabled)
            ToggleOffFlashlight();
        else if (!_headFlashlightBattery.IsDepleted())
            ToggleOnFlashlight();
        else
            _audioPlayer3D.PlayAudio3D(_flashlightOnDischargedAudio);
    }

    private void ToggleOnFlashlight()
    {
        _flashlight.LightEnergy = _lightEnergy;
        _audioPlayer3D.PlayAudio3D(_flashlightOnAudio);
        IsFlashlightEnabled = true;
    }

    private void ToggleOffFlashlight()
    {
        _flashlight.LightEnergy = 0;
        _audioPlayer3D.PlayAudio3D(_flashlightOffAudio);
        IsFlashlightEnabled = false;
    }
}
