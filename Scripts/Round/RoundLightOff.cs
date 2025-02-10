using Godot;
using System;

public partial class RoundLightOff : Node
{
    [Export] private Timer _timer;
    [Export] private Light3D _light;

    [ExportGroup("Audio")]
    [Export] private AudioPlayer3D _audioPlayer3D;
    [Export] private AudioStreamMP3 _lightOffAudio;

	public override void _Ready()
	{
        _timer.Timeout += TurnOffLight;
    }

	private void TurnOffLight()
	{
        _timer.Timeout -= TurnOffLight;
        GlobalRpcFunctions.Instance.SetVisibility(_light.GetPath(), false);
        _audioPlayer3D.PlayAudio3D(_lightOffAudio);
    }
}
