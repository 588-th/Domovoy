using Godot;

public partial class MonsterParticles : Node
{
    [Export] private GpuParticles3D _particles;
    [Export] private Area3D _monsterArea;

    public override void _Ready()
    {
        _monsterArea.AreaEntered += (_) => EnableParticles();
        _monsterArea.AreaExited += (_) => DisableParticles();
    }

    public override void _ExitTree()
    {
        _monsterArea.AreaEntered -= (_) => EnableParticles();
        _monsterArea.AreaExited -= (_) => DisableParticles();
    }

    private void EnableParticles()
    {
        _particles.Emitting = true;
    }

    private void DisableParticles()
    {
        _particles.Emitting = false;
    }
}
