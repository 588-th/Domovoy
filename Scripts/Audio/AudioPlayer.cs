using Godot;

public partial class AudioPlayer : AudioStreamPlayer
{
    [Export] private Node _playerRoot;

    public void PlayAudio(AudioStreamMP3 audio)
    {
        int peerID = int.Parse(_playerRoot.Name);
        if (peerID == 1)
        {
            AudioStreamPlaybackPolyphonic audioStreamPlayback = GetStreamPlayback() as AudioStreamPlaybackPolyphonic;
            audioStreamPlayback.PlayStream(audio);
            return;
        }

        RpcId(peerID, nameof(RpcPlayAudio), GetPath(), audio.ResourcePath);
    }

    [Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void RpcPlayAudio(NodePath streamPlayerPath, string audioPath)
    {
        AudioStreamPlayer streamPlayer = GetNode(streamPlayerPath) as AudioStreamPlayer;
        AudioStreamPlaybackPolyphonic audioStreamPlayback = streamPlayer.GetStreamPlayback() as AudioStreamPlaybackPolyphonic;

        AudioStreamMP3 audio = ResourceLoader.Load<AudioStreamMP3>(audioPath);
        audioStreamPlayback.PlayStream(audio);
    }
}
