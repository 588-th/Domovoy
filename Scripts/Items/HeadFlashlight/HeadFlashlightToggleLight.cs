using Godot;

public partial class HeadFlashlightToggleLight : Node
{
    [Export] private SpotLight3D _flashlight;
    [Export] private HeadFlashlightBattery _headFlashlightBattery;
    [Export] private HeadFlashlightFlicker _headFlashlightFlicker;
    [Export] private float _lightEnergy;

    [Export] private AudioPlayer _audioPlayer;
    [Export] private AudioStreamMP3 _flashlightOnAudio;
    [Export] private AudioStreamMP3 _flashlightOffAudio;
    [Export] private AudioStreamMP3 _flashlightOnDischargedAudio;

    private bool _isFlashlightEnabled;

    public override void _Process(double delta)
    {
        if (_isFlashlightEnabled)
        {
            _headFlashlightBattery.Drain((float)delta);

            if (_headFlashlightBattery.IsDepleted())
            {
                ToggleOffFlashlight();
                return;
            }

            _headFlashlightFlicker.HandleFlicker(_headFlashlightBattery.GetBatteryLife(), (float)delta);
        }
    }

    public void ToggleFlashlight()
    {
        if (_isFlashlightEnabled)
            ToggleOffFlashlight();
        else if (!_headFlashlightBattery.IsDepleted())
            ToggleOnFlashlight();
        else
            _audioPlayer.PlayAudio3D(_flashlightOnDischargedAudio);
    }

    private void ToggleOnFlashlight()
    {
        _flashlight.LightEnergy = _lightEnergy;
        _audioPlayer.PlayAudio3D(_flashlightOnAudio);
        _isFlashlightEnabled = true;
    }

    private void ToggleOffFlashlight()
    {
        _flashlight.LightEnergy = 0;
        _audioPlayer.PlayAudio3D(_flashlightOffAudio);
        _isFlashlightEnabled = false;
    }
}
