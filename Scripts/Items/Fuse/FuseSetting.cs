using Godot;
using System;

public partial class FuseSetting : Node
{
    [Export] private Fuse _fuse;

    public Action FuseUsed;

    public void Setting()
    {
        var rayOfLookMeta = _fuse.HoldingPlayer.GetMeta("RayOfLook");
        RayCast3D rayOfLook = _fuse.HoldingPlayer.GetNode(rayOfLookMeta.ToString()) as RayCast3D;
        Node collider = rayOfLook.GetCollider() as Node;
        if (collider == null)
            return;

        if (!collider.IsInGroup("Object:Shield"))
            return;

        var shieldFuseSettingMeta = collider.GetMeta("ShieldFuseSetting");
        Node node = collider.GetNode(shieldFuseSettingMeta.ToString());
        if (node == null)
            return;

        ShieldFuseSetting shieldFuseSetting = node as ShieldFuseSetting;
        shieldFuseSetting.SettingFuse();

        FuseUsed?.Invoke();
    }
}
