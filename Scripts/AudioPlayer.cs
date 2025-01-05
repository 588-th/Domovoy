using Godot;

public partial class AudioPlayer : Node
{
    [Export] private AudioStreamPlayer _audioStreamPlayer;
    [Export] private AudioStreamPlayer3D _audioStreamPlayer3D;

    public void PlayAudio(AudioStreamMP3 audio)
    {
        _audioStreamPlayer.Stream = audio;
        _audioStreamPlayer.Play();
    }

    public void PlayAudio3DLocaly(AudioStreamMP3 audio)
    {
        _audioStreamPlayer3D.Stream = audio;
        _audioStreamPlayer3D.Play();
    }

    public void PlayAudio3DForAll(AudioStreamMP3 audio, bool local)
    {
        if (local)
            Rpc(nameof(PlaySoundForAll), audio.ResourcePath);
        else
            Rpc(nameof(PlaySoundForAllExceptLocal), audio.ResourcePath);
    }

    [Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void PlaySoundForAll(string audioPath)
    {
        AudioStreamMP3 audio = ResourceLoader.Load<AudioStreamMP3>(audioPath);
        _audioStreamPlayer3D.Stream = audio;
        _audioStreamPlayer3D.Play();
    }

    [Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    private void PlaySoundForAllExceptLocal(string audioPath)
    {
        AudioStreamMP3 audio = ResourceLoader.Load<AudioStreamMP3>(audioPath);
        _audioStreamPlayer3D.Stream = audio;
        _audioStreamPlayer3D.Play();
    }
}
