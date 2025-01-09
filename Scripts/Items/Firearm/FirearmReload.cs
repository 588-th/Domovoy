using Godot;

public partial class FirearmReload : Node
{
    [Export] private Firearm _firearm;
    [Export] private FirearmParameters _firearmParameters;
    [Export] private AudioPlayer _audioPlayer;

    public void Reload()
    {
        if (_firearmParameters.CurrentClips <= 0)
            return;

        _firearmParameters.CurrentClips--;
        _firearmParameters.CurrentBulletsInClip = _firearmParameters.BulletsPerClip;
        _audioPlayer.PlayAudio3D(_firearmParameters.ReloadAudio);
        _firearm.ItemUsed?.Invoke();
    }
}
