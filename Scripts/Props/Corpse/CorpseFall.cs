using Godot;

public partial class CorpseFall : Node
{
    [Export] private RigidBody3D _corpse;

    [Export] private AudioPlayer _audioPlayer;
    [Export] private AudioStreamMP3 _bodyFallAudio;

    public override void _Ready()
    {
        _corpse.BodyEntered += OnCorpseFall;
    }

    public override void _ExitTree()
    {
        _corpse.BodyEntered -= OnCorpseFall;
    }

    private void OnCorpseFall(Node body)
    {
        _audioPlayer.PlayAudio3D(_bodyFallAudio);
    }
}
