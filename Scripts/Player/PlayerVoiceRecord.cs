using Godot;
using System;
using System.IO;
using System.IO.Compression;

public partial class PlayerVoiceRecord : Node
{
    [Export] private InputActions _inputActions;
    [Export] private PlayerVoicePlayer _playerVoicePlayer;
    [Export] private string _busName;

    private AudioEffectCapture _audioEffectCapture;
    private bool _isMicrophoneEnabled;

    public override void _Ready()
    {
        _inputActions.ToggleMicrophoneDown += ToggleMicrophone;
        Init();
    }

    public override void _ExitTree()
    {
        _inputActions.ToggleMicrophoneDown -= ToggleMicrophone;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (!_isMicrophoneEnabled)
            return;

        int availableFrames = _audioEffectCapture.GetFramesAvailable();
        if (availableFrames > 0)
        {
            byte[] compressedData = CompressFloatArray(GetMicrophoneData(availableFrames));
            _playerVoicePlayer.SendMicrophoneData(compressedData);
        }
    }

    private void Init()
    {
        int busIndex = AudioServer.GetBusIndex(_busName);
        _audioEffectCapture = AudioServer.GetBusEffect(busIndex, 0) as AudioEffectCapture;
    }

    private void ToggleMicrophone()
    {
        _isMicrophoneEnabled = !_isMicrophoneEnabled;
    }

    private float[] GetMicrophoneData(int frames)
    {
        Vector2[] stereoData = _audioEffectCapture.GetBuffer(frames);

        float[] monoData = new float[stereoData.Length];
        Span<float> monoSpan = monoData.AsSpan();

        for (int i = 0; i < stereoData.Length; i++)
        {
            monoSpan[i] = (stereoData[i].X + stereoData[i].Y) * 0.5f;
        }

        return monoData;
    }

    private byte[] CompressFloatArray(float[] floatArray)
    {
        byte[] byteArray = new byte[floatArray.Length * sizeof(float)];
        Buffer.BlockCopy(floatArray, 0, byteArray, 0, byteArray.Length);

        using MemoryStream memoryStream = new();
        using (GZipStream gZipStream = new(memoryStream, CompressionMode.Compress, true))
        {
            gZipStream.Write(byteArray, 0, byteArray.Length);
            gZipStream.Flush();
        }

        return memoryStream.ToArray();
    }
}
