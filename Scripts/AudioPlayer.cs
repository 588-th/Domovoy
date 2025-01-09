using Godot;
using System.Linq;

public partial class AudioPlayer : Node
{
    [Export] private Node _playerRoot;
    [Export] private AudioStreamPlayer _audioStreamPlayer;
    [Export] private AudioStreamPlayer3D _audioStreamPlayer3D;

    public void PlayAudio(AudioStreamMP3 audio)
    {
        int clientPeer = int.Parse(_playerRoot.Name);
        if (clientPeer == 1)
        {
            _audioStreamPlayer.Stream = audio;
            _audioStreamPlayer.Play();
        }
        else
            RpcId(clientPeer, nameof(RpcPlayAudio), audio.ResourcePath);
    }

    public void PlayAudio3D(AudioStreamMP3 audio)
    {
        Rpc(nameof(RpcPlayAudio3D), audio.ResourcePath);
        _audioStreamPlayer3D.Stream = audio;
        _audioStreamPlayer3D.Play();
    }

    public void PlayAudio3DExceptClient(AudioStreamMP3 audio)
    {
        int clientPeer = int.Parse(_playerRoot.Name);

        int[] allPeers = Multiplayer.GetPeers().ToArray().Union(new int[] { 1 }).ToArray(); ;
        int[] targetPeers = allPeers.Except(new int[] { clientPeer }).ToArray();

        foreach (int peer in targetPeers)
        {
            if (peer == 1)
            {
                _audioStreamPlayer3D.Stream = audio;
                _audioStreamPlayer3D.Play();
            }
            else
                RpcId(peer, nameof(RpcPlayAudio3D), audio.ResourcePath);
        }
    }

    [Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void RpcPlayAudio(string audioPath)
    {
        AudioStreamMP3 audio = ResourceLoader.Load<AudioStreamMP3>(audioPath);
        _audioStreamPlayer.Stream = audio;
        _audioStreamPlayer.Play();
    }

    [Rpc(MultiplayerApi.RpcMode.Authority, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void RpcPlayAudio3D(string audioPath)
    {
        AudioStreamMP3 audio = ResourceLoader.Load<AudioStreamMP3>(audioPath);
        _audioStreamPlayer3D.Stream = audio;
        _audioStreamPlayer3D.Play();
    }
}
