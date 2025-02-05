using Godot;
using System;
using System.IO;
using System.IO.Compression;
using System.Collections.Generic;

public partial class PlayerVoicePlayer : AudioStreamPlayer3D
{
    private AudioStreamGeneratorPlayback _voicePlayback;
    private List<float> _receiveBuffer = new();
    private const int MaxBufferSize = 48000;

    public override void _Ready()
    {
        _voicePlayback = GetStreamPlayback() as AudioStreamGeneratorPlayback;
    }

    public override void _PhysicsProcess(double delta)
    {
        PlayMicrophoneData();
    }

    private void PlayMicrophoneData()
    {
        if (_receiveBuffer.Count == 0 || _voicePlayback == null)
            return;

        int framesAvailable = _voicePlayback.GetFramesAvailable();
        if (framesAvailable == 0)
            return;

        int framesToPlay = Mathf.Min(framesAvailable, _receiveBuffer.Count);
        if (framesToPlay == 0)
            return;

        for (int i = 0; i < framesToPlay; i++)
        {
            float sample = _receiveBuffer[i];
            _voicePlayback.PushFrame(new Vector2(sample, sample));
        }

        _receiveBuffer.RemoveRange(0, framesToPlay);

        if (_receiveBuffer.Count > MaxBufferSize)
        {
            GD.Print($"[Warning] Buffer overflow: dropping {_receiveBuffer.Count - MaxBufferSize} samples.");
            _receiveBuffer.RemoveRange(0, _receiveBuffer.Count - MaxBufferSize);
        }
    }

    public void SendMicrophoneData(byte[] compressedMicrophoneData)
    {
        if (compressedMicrophoneData == null || compressedMicrophoneData.Length == 0)
            return;

        Rpc(nameof(RpcSendMicrophoneData), compressedMicrophoneData);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = false, TransferMode = MultiplayerPeer.TransferModeEnum.Unreliable)]
    private void RpcSendMicrophoneData(byte[] compressedMicrophoneData)
    {
        float[] microphoneData = DecompressFloatArray(compressedMicrophoneData);
        if (microphoneData.Length > 0)
        {
            _receiveBuffer.AddRange(microphoneData);
        }
    }

    private float[] DecompressFloatArray(byte[] compressedArray)
    {
        using MemoryStream memoryStream = new(compressedArray);
        using GZipStream gZipStream = new(memoryStream, CompressionMode.Decompress);
        using MemoryStream resultStream = new();

        gZipStream.CopyTo(resultStream);
        byte[] byteArray = resultStream.ToArray();

        if (byteArray.Length == 0)
            return Array.Empty<float>();

        float[] floatArray = new float[byteArray.Length / sizeof(float)];
        Buffer.BlockCopy(byteArray, 0, floatArray, 0, byteArray.Length);

        return floatArray;
    }
}
