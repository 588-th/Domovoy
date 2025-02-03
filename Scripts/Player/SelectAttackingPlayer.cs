using Godot;

public partial class SelectAttackingPlayer : Node
{
    [Export] private MeleeAttackActions _meleeAttackActions;
    [Export] private RayCast3D _rayOfAttack;

    [ExportGroup("Parameters")]
    [Export] private ShaderMaterial _attackSelecting;
    [Export] private Color _attackColor;
    [Export] private Color _cooldownColor;

    private Node3D _lastSelectedPlayer = null;
    private Material _originalMaterial = null;
    private bool _isCooldown = false;

    public override void _Ready()
    {
        _meleeAttackActions.CooldownStart += ChangeMaterialColor;
        _meleeAttackActions.CooldownEnd += ChangeMaterialColor;
    }

    public override void _ExitTree()
    {
        _meleeAttackActions.CooldownStart -= ChangeMaterialColor;
        _meleeAttackActions.CooldownEnd -= ChangeMaterialColor;
    }

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
            ApplyShader(collider, _attackSelecting);
            _lastSelectedPlayer = collider;
        }
    }

    private void ApplyShader(Node3D player, Material material)
    {
        if (player.GetNodeOrNull<MeshInstance3D>("Model") is MeshInstance3D mesh)
        {
            if (_originalMaterial == null)
                _originalMaterial = mesh.MaterialOverlay;

            mesh.MaterialOverlay = material;
        }
    }

    private void RemoveShader(Node3D player)
    {
        if (player.GetNodeOrNull<MeshInstance3D>("Model") is MeshInstance3D mesh)
            mesh.MaterialOverlay = _originalMaterial;
    }

    private void ChangeMaterialColor()
    {
        if (_isCooldown)
            _attackSelecting.SetShaderParameter("color", _attackColor);
        else
            _attackSelecting.SetShaderParameter("color", _cooldownColor);

        _isCooldown = !_isCooldown;
    }
}
