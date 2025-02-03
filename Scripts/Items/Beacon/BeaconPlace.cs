using Godot;

public partial class BeaconPlace : Node
{
    [Export] private Beacon _beacon;
    [Export] private RigidBody3D _beaconBody;
    [Export] private OmniLight3D _light;
    [Export] private float _lightEnergy;
    [Export] private AudioPlayer3D _audioPlayer3D;
    [Export] private AudioStreamMP3 _beaconNoise;

    public void Place()
    {
        var rayOfLookMeta = _beacon.HoldingPlayer.GetMeta("RayOfLook");
        RayCast3D rayOfLook = _beacon.HoldingPlayer.GetNode(rayOfLookMeta.ToString()) as RayCast3D;
        if (rayOfLook == null || !rayOfLook.IsColliding())
            return;

        CollisionObject3D collider = rayOfLook.GetCollider() as CollisionObject3D;
        int LayerEnvironment = 1;
        if (collider == null || !collider.GetCollisionLayerValue(LayerEnvironment))
            return;

        var playerHotbarMeta = _beacon.HoldingPlayer.GetMeta("PlayerHotbar");
        PlayerHotbar playerHotbar = _beacon.HoldingPlayer.GetNode(playerHotbarMeta.ToString()) as PlayerHotbar;
        if (playerHotbar == null)
            return;

        playerHotbar.AbortItem(_beacon);
        SetTransform(rayOfLook);
        _audioPlayer3D.PlayAudio3D(_beaconNoise);

        _light.LightEnergy = _lightEnergy;
        GlobalRpcFunctions.Instance.RemoveGroup(_beaconBody.GetPath(), "Object:Item");
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

        _beaconBody.GlobalTransform = new Transform3D(basis, collisionPoint);
    }
}
