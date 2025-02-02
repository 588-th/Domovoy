using Godot;

public partial class SelectAttackingPlayer : Node
{
    [Export] private RayCast3D _rayOfAttack;
    [Export] private ShaderMaterial _attackSelecting;

    private Node3D _lastSelectedPlayer = null;
    private Material _originalMaterial = null;

    public override void _PhysicsProcess(double delta)
    {
        CheckColliding();
    }

    private void CheckColliding()
    {
        if (_lastSelectedPlayer != null)
        {
            RemoveShader(_lastSelectedPlayer);
            _lastSelectedPlayer = null;
        }

        if (_rayOfAttack.GetCollider() is Node3D collider && collider.IsInGroup("Hitbox:Player"))
        {
            ApplyShader(collider);
            _lastSelectedPlayer = collider;
        }
    }

    private void ApplyShader(Node3D player)
    {
        if (player.GetNodeOrNull<MeshInstance3D>("Model") is MeshInstance3D mesh)
        {
            if (_originalMaterial == null)
            {
                _originalMaterial = mesh.MaterialOverlay;
            }

            mesh.MaterialOverlay = _attackSelecting;
        }
    }

    private void RemoveShader(Node3D player)
    {
        if (player.GetNodeOrNull<MeshInstance3D>("Model") is MeshInstance3D mesh)
        {
            mesh.MaterialOverlay = _originalMaterial;
        }
    }
}
