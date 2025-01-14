using Godot;
using System;

public partial class PlayerFlashlight : Node
{
    [Export] private InputActions _inputActions;
    [Export] private SpotLight3D _flashlight;
    [Export] private SpotLight3D _flashlightSecond;
    [Export] private Area3D _flashlightArea;
    [Export] private float _flashlightEnergy;
    [Export] private float _flashlightEnergySecond;
    [Export] private float _batteryLife = 100f;
    [Export] private float _batteryDrainRate = 5f;
    [Export] private float _blinkThreshold = 50f;
    [Export] private float _maxBlinkInterval = 0.5f;
    [Export] private float _minBlinkInterval = 0.1f;

    [Export] private AudioPlayer _audioPlayer;
    [Export] private AudioStreamMP3 _flashlightOnAudio;
    [Export] private AudioStreamMP3 _flashlightOffAudio;

    private bool _isFlashlightEnable;
    private Random _random = new Random();
    private float _timeUntilNextBlink = 0f;

    public override void _Ready()
    {
        _inputActions.ToggleFlashlightDown += ToggleFlashlight;
    }

    public override void _ExitTree()
    {
        _inputActions.ToggleFlashlightDown -= ToggleFlashlight;
    }

    public override void _Process(double delta)
    {
        if (_isFlashlightEnable)
        {
            DrainBattery((float)delta);

            if (_batteryLife <= 0)
            {
                ToggleOffFlashlight();
                return;
            }

            if (_batteryLife <= _blinkThreshold)
            {
                HandleFlicker((float)delta);
            }
        }
    }

    private void DrainBattery(float delta)
    {
        _batteryLife -= _batteryDrainRate * delta;
        _batteryLife = Mathf.Max(_batteryLife, 0);
    }

    private void HandleFlicker(float delta)
    {
        _timeUntilNextBlink -= delta;

        if (_timeUntilNextBlink <= 0)
        {
            float blinkInterval = Mathf.Lerp(_maxBlinkInterval, _minBlinkInterval, 1f - (_batteryLife / _blinkThreshold));
            _timeUntilNextBlink = blinkInterval;

            FlickerLight();
        }
    }

    private void FlickerLight()
    {
        float randomFactor = (float)_random.NextDouble();
        float intensityFactor = Mathf.Lerp(0.5f, 1.2f, randomFactor); 
        _flashlight.LightEnergy = _flashlightEnergy * intensityFactor;
        _flashlightSecond.LightEnergy = _flashlightEnergySecond * intensityFactor;

        Timer resetTimer = new Timer();
        AddChild(resetTimer);
        resetTimer.WaitTime = 0.05f;
        resetTimer.OneShot = true;
        resetTimer.Timeout += () =>
        {
            _flashlight.LightEnergy = _flashlightEnergy;
            _flashlightSecond.LightEnergy = _flashlightEnergySecond;
            resetTimer.QueueFree();
        };
        resetTimer.Start();
    }

    private void ToggleFlashlight()
    {
        if (_isFlashlightEnable)
            ToggleOffFlashlight();
        else if (_batteryLife > 0)
            ToggleOnFlashlight();
    }

    private void ToggleOnFlashlight()
    {
        _flashlight.LightEnergy = _flashlightEnergy;
        _flashlightSecond.LightEnergy = _flashlightEnergySecond;
        _flashlightArea.Monitorable = true;
        _audioPlayer.PlayAudio(_flashlightOnAudio);
        _audioPlayer.PlayAudio3DExceptClient(_flashlightOnAudio);
        _isFlashlightEnable = true;
    }

    private void ToggleOffFlashlight()
    {
        _flashlight.LightEnergy = 0;
        _flashlightSecond.LightEnergy = 0;
        _flashlightArea.Monitorable = false;
        _audioPlayer.PlayAudio(_flashlightOffAudio);
        _audioPlayer.PlayAudio3DExceptClient(_flashlightOffAudio);
        _isFlashlightEnable = false;
    }
}
