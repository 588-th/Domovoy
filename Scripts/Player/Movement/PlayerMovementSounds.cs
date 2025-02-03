using Godot;

public partial class PlayerMovementSounds : Node
{
    [Export] private MovementActions _movementActions;
    [Export] private Timer _stepTimer;

    [ExportGroup("Parameters")]
    [Export] private float _stepInterval;
    [Export] public bool PlayAudioOnlyLocaly;

    [ExportGroup("Audio")]
    [Export] private AudioPlayer3D _audioPlayer3D;
    [Export] private AudioStreamMP3 _stepAudio;
    [Export] private AudioStreamMP3 _jumpAudio;
    [Export] private AudioStreamMP3 _landAudio;

    private bool _isWalking = false;

    public override void _Ready()
    {
        _stepTimer.WaitTime = _stepInterval;
        _stepTimer.OneShot = false;
        _stepTimer.Autostart = false;

        _movementActions.IsWalkState += StartStepAudioLoop;
        _movementActions.IsNotWalkState += StopStepAudioLoop;
        _movementActions.IsJumpState += PlayJumpAudio;
        _movementActions.IsNotJumpState += PlayLandAudio;
        _stepTimer.Timeout += PlayStepAudio;
    }

    public override void _ExitTree()
    {
        _movementActions.IsWalkState -= StartStepAudioLoop;
        _movementActions.IsNotWalkState -= StopStepAudioLoop;
        _movementActions.IsJumpState -= PlayJumpAudio;
        _movementActions.IsNotJumpState -= PlayLandAudio;
        _stepTimer.Timeout -= PlayStepAudio;
    }

    private void StartStepAudioLoop()
    {
        if (_isWalking)
            return;

        _isWalking = true;
        _stepTimer.Start();
    }

    private void StopStepAudioLoop()
    {
        _isWalking = false;
        _stepTimer.Stop();
    }

    private void PlayStepAudio()
    {
        if (!_isWalking)
            return;

        _audioPlayer3D.PlayAudio3D(_stepAudio, PlayAudioOnlyLocaly);
        _stepTimer.Start();
    }

    private void PlayJumpAudio()
    {
        _audioPlayer3D.PlayAudio3D(_jumpAudio, PlayAudioOnlyLocaly);
    }

    private void PlayLandAudio()
    {
        _audioPlayer3D.PlayAudio3D(_landAudio, PlayAudioOnlyLocaly);
    }
}
