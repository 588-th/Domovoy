using Godot;

public partial class FirearmToggleAutomatic : Node
{
    [Export] private FirearmParameters _firearmParameters;
    [Export] private AudioPlayer _audioPlayer;

    public void ToggleAutomatic()
    {
        if (_firearmParameters.IsCanSwitchAitomatic)
        {
            _firearmParameters.IsAutomatic = !_firearmParameters.IsAutomatic;
            _audioPlayer.PlayAudio3D(_firearmParameters.ToggleAutomaticAudio);
        }
    }
}
