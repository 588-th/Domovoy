using Godot;
using System.Collections.Generic;

public partial class AudioPlayer3D : AudioStreamPlayer3D
{
    [Export] private Node _playerRoot;

    private Dictionary<string, long> _playingSounds = new();

    public void PlayAudio3D(AudioStreamMP3 audio, bool onlyLocal = false)
    {
        if (!onlyLocal)
        {
            Rpc(nameof(RpcPlayAudio3D), GetPath(), audio.ResourcePath);
            return;
        }

        int peerID = int.Parse(_playerRoot.Name);
        if (peerID != 1)
        {
            RpcId(peerID, nameof(RpcPlayAudio3D), GetPath(), audio.ResourcePath);
            return;
        }

        AudioStreamPlaybackPolyphonic audioStreamPlayback = GetStreamPlayback() as AudioStreamPlaybackPolyphonic;
        long voiceId = audioStreamPlayback.PlayStream(audio);
        _playingSounds[audio.ResourcePath] = voiceId;
    }

    public void StopAudio3D(AudioStreamMP3 audio, bool onlyLocal = false)
    {
        if (!onlyLocal)
        {
            Rpc(nameof(RpcStopAudio3D), GetPath(), audio.ResourcePath);
            return;
        }

        int peerID = int.Parse(_playerRoot.Name);
        if (peerID != 1)
        {
            RpcId(peerID, nameof(RpcStopAudio3D), GetPath(), audio.ResourcePath);
            return;
        }

        AudioStreamPlaybackPolyphonic audioStreamPlayback = GetStreamPlayback() as AudioStreamPlaybackPolyphonic;
        _playingSounds.TryGetValue(audio.ResourcePath, out long voiceId);
        audioStreamPlayback.StopStream(voiceId);
        _playingSounds.Remove(audio.ResourcePath);
    }

    [Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void RpcStopAudio3D(NodePath streamPlayer3DPath, string audioPath)
    {
        AudioStreamPlayer3D streamPlayer3D = GetNode<AudioStreamPlayer3D>(streamPlayer3DPath);
        AudioStreamPlaybackPolyphonic audioStreamPlayback = streamPlayer3D?.GetStreamPlayback() as AudioStreamPlaybackPolyphonic;

        if (audioStreamPlayback != null && _playingSounds.TryGetValue(audioPath, out long voiceId))
        {
            audioStreamPlayback.StopStream(voiceId);
            _playingSounds.Remove(audioPath);
        }
    }

    [Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void RpcPlayAudio3D(NodePath streamPlayer3DPath, string audioPath)
    {
        AudioStreamPlayer3D streamPlayer3D = GetNode<AudioStreamPlayer3D>(streamPlayer3DPath);
        AudioStreamPlaybackPolyphonic audioStreamPlayback = streamPlayer3D?.GetStreamPlayback() as AudioStreamPlaybackPolyphonic;

        if (audioStreamPlayback != null)
        {
            AudioStreamMP3 audio = ResourceLoader.Load<AudioStreamMP3>(audioPath);
            long voiceId = audioStreamPlayback.PlayStream(audio);
            _playingSounds[audioPath] = voiceId;
        }
    }
}
