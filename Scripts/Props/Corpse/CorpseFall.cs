using Godot;

public partial class CorpseFall : Node
{
    [Export] private RigidBody3D _corpse;

    [Export] private AudioPlayer3D _audioPlayer3D;
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
        _audioPlayer3D.PlayAudio3D(_bodyFallAudio);
    }
}
