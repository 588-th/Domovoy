using Godot;
using System.Collections.Generic;

public partial class AudioPlayer3D : AudioStreamPlayer3D
{
    private Dictionary<string, long> playingVoices = new();

    public void PlayAudio3D(AudioStreamMP3 audio, bool onlyLocal = false)
    {
        if (onlyLocal)
        {
            AudioStreamPlaybackPolyphonic audioStreamPlayback = GetStreamPlayback() as AudioStreamPlaybackPolyphonic;
            if (audioStreamPlayback != null)
            {
                long voiceId = audioStreamPlayback.PlayStream(audio);
                playingVoices[audio.ResourcePath] = voiceId;
            }
            return;
        }

        Rpc(nameof(RpcPlayAudio3D), GetPath(), audio.ResourcePath);
    }

    public void StopAudio3D(AudioStreamMP3 audio, bool onlyLocal = false)
    {
        if (onlyLocal)
        {
            AudioStreamPlaybackPolyphonic audioStreamPlayback = GetStreamPlayback() as AudioStreamPlaybackPolyphonic;
            if (audioStreamPlayback != null && playingVoices.TryGetValue(audio.ResourcePath, out long voiceId))
            {
                audioStreamPlayback.StopStream(voiceId);
                playingVoices.Remove(audio.ResourcePath);
            }
            return;
        }

        Rpc(nameof(RpcStopAudio3D), GetPath(), audio.ResourcePath);
    }

    [Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void RpcStopAudio3D(NodePath streamPlayer3DPath, string audioPath)
    {
        AudioStreamPlayer3D streamPlayer3D = GetNode<AudioStreamPlayer3D>(streamPlayer3DPath);
        AudioStreamPlaybackPolyphonic audioStreamPlayback = streamPlayer3D?.GetStreamPlayback() as AudioStreamPlaybackPolyphonic;

        if (audioStreamPlayback != null && playingVoices.TryGetValue(audioPath, out long voiceId))
        {
            audioStreamPlayback.StopStream(voiceId);
            playingVoices.Remove(audioPath);
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
            playingVoices[audioPath] = voiceId;
        }
    }
}
