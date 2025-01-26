using Godot;

public partial class PlayerMovementSounds : Node
{
    [Export] private MovementActions _movementActions;
    [Export] private AudioPlayer _audioPlayer;
    [Export] private Timer _stepTimer;
    [Export] private float _stepInterval;
    [Export] private AudioStreamMP3 _stepAudio;
    [Export] private AudioStreamMP3 _jumpAudio;
    [Export] private AudioStreamMP3 _landAudio;
    [Export] public bool PlayAudioOnlyLocaly;

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

        _audioPlayer.PlayAudio(_stepAudio);
        if (!PlayAudioOnlyLocaly)
            _audioPlayer.PlayAudio3DExceptClient(_stepAudio);

        _stepTimer.Start();
    }

    private void PlayJumpAudio()
    {
        _audioPlayer.PlayAudio(_jumpAudio);
        if (!PlayAudioOnlyLocaly)
            _audioPlayer.PlayAudio3DExceptClient(_jumpAudio);
    }

    private void PlayLandAudio()
    {
        _audioPlayer.PlayAudio(_landAudio);
        if (!PlayAudioOnlyLocaly)
            _audioPlayer.PlayAudio3DExceptClient(_landAudio);
    }
}
