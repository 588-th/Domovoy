using Godot;

public partial class FirearmToggleAutomatic : Node
{
    [Export] private FirearmParameters _firearmParameters;
    [Export] private AudioPlayer3D _audioPlayer3D;

    public void ToggleAutomatic()
    {
        if (_firearmParameters.IsCanSwitchAitomatic)
        {
            _firearmParameters.IsAutomatic = !_firearmParameters.IsAutomatic;
            _audioPlayer3D.PlayAudio3D(_firearmParameters.ToggleAutomaticAudio);
        }
    }
}
