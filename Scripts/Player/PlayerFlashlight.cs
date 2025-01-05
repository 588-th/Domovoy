using Godot;

public partial class PlayerFlashlight : Node
{
    [Export] private InputActions _inputActions;
    [Export] private SpotLight3D _flashlight;
    [Export] private Area3D _flashlightArea;
    [Export] private float _flashlightEnergy;

    [Export] private AudioPlayer _audioPlayer;
    [Export] private AudioStreamMP3 _flashlightOnAudio;
    [Export] private AudioStreamMP3 _flashlightOffAudio;

    private bool _isFlashlightEnable;

    public override void _Ready()
    {
        _inputActions.ToggleFlashlightDown += ToggleFlashlight;
    }

    public override void _ExitTree()
    {
        _inputActions.ToggleFlashlightDown -= ToggleFlashlight;
    }

    private void ToggleFlashlight()
    {
        if (_isFlashlightEnable)
            ToggleOffFlashlight();
        else
            ToggleOnFlashlight();
    }

    private void ToggleOnFlashlight()
    {
        _flashlight.LightEnergy = _flashlightEnergy;
        _flashlightArea.Monitorable = true;
        _audioPlayer.PlayAudio(_flashlightOnAudio);
        _audioPlayer.PlayAudio3DForAll(_flashlightOnAudio, false);
        _isFlashlightEnable = true;
    }

    private void ToggleOffFlashlight()
    {
        _flashlight.LightEnergy = 0;
        _flashlightArea.Monitorable = false;
        _audioPlayer.PlayAudio(_flashlightOffAudio);
        _audioPlayer.PlayAudio3DForAll(_flashlightOffAudio, false);
        _isFlashlightEnable = false;
    }
}
