using Godot;

public partial class MonsterParticles : Node
{
    [Export] private GpuParticles3D _particles;
    [Export] private Area3D _monsterArea;

    public override void _Ready()
    {
        _monsterArea.AreaEntered += EnableParticles;
        _monsterArea.AreaExited += DisableParticles;
    }

    public override void _ExitTree()
    {
        _monsterArea.AreaEntered -= EnableParticles;
        _monsterArea.AreaExited -= DisableParticles;
    }

    private void EnableParticles(Node area)
    {
        _particles.Emitting = true;
    }

    private void DisableParticles(Node area)
    {
        _particles.Emitting = false;
    }
}
