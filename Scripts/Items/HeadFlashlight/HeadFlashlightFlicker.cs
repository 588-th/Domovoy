using Godot;
using System;

public partial class HeadFlashlightFlicker : Node
{
    [Export] public SpotLight3D _flashlight;
    [Export] public float BlinkThreshold = 50f;
    [Export] public float MaxBlinkInterval = 0.5f;
    [Export] public float MinBlinkInterval = 0.1f;

    private readonly Random _random = new();
    private float _timeUntilNextBlink = 0f;

    public void HandleFlicker(float batteryLife, float delta)
    {
        if (batteryLife > BlinkThreshold)
            return;

        _timeUntilNextBlink -= delta;
        if (_timeUntilNextBlink <= 0)
        {
            float blinkInterval = Mathf.Lerp(MaxBlinkInterval, MinBlinkInterval, 1f - (batteryLife / BlinkThreshold));
            _timeUntilNextBlink = blinkInterval;

            Flicker();
        }
    }

    private void Flicker()
    {
        float randomFactor = (float)_random.NextDouble();
        float intensityFactor = Mathf.Lerp(0.5f, 1.2f, randomFactor);
        _flashlight.LightEnergy = intensityFactor;

        Timer resetTimer = new();
        _flashlight.AddChild(resetTimer);
        resetTimer.WaitTime = 0.05f;
        resetTimer.OneShot = true;
        resetTimer.Timeout += () =>
        {
            _flashlight.LightEnergy = 1f;
            resetTimer.QueueFree();
        };
        resetTimer.Start();
    }
}
