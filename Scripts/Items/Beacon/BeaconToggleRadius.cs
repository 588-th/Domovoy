using Godot;

public partial class BeaconToggleRadius : Node
{
    [Export] private Beacon _beacon;
    [Export] private MeshInstance3D _radius;

    private bool _isRadiusEnable;

    public override void _ExitTree()
    {
        ToggleOffRadius();
    }

    public override void _Process(double delta)
    {
        if (_isRadiusEnable)
            UpdateRadiusPosition();
    }

    public void ToggleRadius()
    {
        if (_isRadiusEnable)
            ToggleOffRadius();
        else
            ToggleOnRadius();
    }

    public void ToggleOnRadius()
    {
        _isRadiusEnable = true;
    }

    public void ToggleOffRadius()
    {
        _radius.Visible = false;
        _isRadiusEnable = false;
    }

    private void UpdateRadiusPosition()
    {
        var rayOfLookMeta = _beacon.HoldingPlayer.GetMeta("RayOfLook");
        RayCast3D rayOfLook = _beacon.HoldingPlayer.GetNode(rayOfLookMeta.ToString()) as RayCast3D;
        if (rayOfLook == null || !rayOfLook.IsColliding())
        {
            _radius.Visible = false;
            return;
        }

        CollisionObject3D collider = rayOfLook.GetCollider() as CollisionObject3D;
        int LayerEnvironment = 1;
        if (collider == null || !collider.GetCollisionLayerValue(LayerEnvironment))
        {
            _radius.Visible = false;
            return;
        }

        SetTransform(rayOfLook);
        _radius.Visible = true;
    }

    private void SetTransform(RayCast3D rayOfLook)
    {
        Vector3 collisionPoint = rayOfLook.GetCollisionPoint();
        Vector3 collisionNormal = rayOfLook.GetCollisionNormal();

        Vector3 up = Vector3.Up;

        if (collisionNormal.IsEqualApprox(Vector3.Up) || collisionNormal.IsEqualApprox(Vector3.Down))
            up = Vector3.Forward;

        Vector3 x = up.Cross(collisionNormal).Normalized();
        if (x.IsEqualApprox(Vector3.Zero))
            x = Vector3.Right;

        Vector3 z = x.Cross(collisionNormal).Normalized();
        Vector3 y = collisionNormal;

        Basis basis = new Basis(x, y, z);

        _radius.GlobalTransform = new Transform3D(basis, collisionPoint);
    }
}
