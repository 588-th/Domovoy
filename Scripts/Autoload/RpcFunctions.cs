using Godot;

public partial class RpcFunctions : Node
{
    public static RpcFunctions Instance { get; private set; }

    public override void _Ready()
    {
        Instance = this;
    }

    public void Reparent(NodePath nodePath, NodePath parentNodePath)
    {
        if (Multiplayer.GetUniqueId() != 1)
            return;

        Rpc(nameof(RpcReparent), nodePath, parentNodePath);
    }

    public void AddGroup(NodePath nodePath, string group)
    {
        if (Multiplayer.GetUniqueId() != 1)
            return;

        Rpc(nameof(RpcAddGroup), nodePath, group);
    }

    public void RemoveGroup(NodePath nodePath, string group)
    {
        if (Multiplayer.GetUniqueId() != 1)
            return;

        Rpc(nameof(RpcRemoveGroup), nodePath, group);
    }

    public void SetCullMaskCamera3D(NodePath camera3DPath, int maskNumber, bool isEnable)
    {
        if (Multiplayer.GetUniqueId() != 1)
            return;

        Rpc(nameof(RpcSetCullMaskCamera3D), camera3DPath, maskNumber, isEnable);
    }

    public void ChangeAlbedoOfGeometry(NodePath nodePath, Color color)
    {
        if (Multiplayer.GetUniqueId() != 1)
            return;

        Rpc(nameof(RpcChangeAlbedoOfGeometry), nodePath, color);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void RpcReparent(NodePath nodePath, NodePath parentNodePath)
    {
        GetNode(nodePath).Reparent(GetNode(parentNodePath));
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void RpcAddGroup(NodePath nodePath, string group)
    {
        GetNode(nodePath).AddToGroup(group);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void RpcRemoveGroup(NodePath nodePath, string group)
    {
        GetNode(nodePath).RemoveFromGroup(group);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void RpcSetCullMaskCamera3D(NodePath camera3DPath, int maskNumber, bool isEnable)
    {
        Camera3D camera3D = GetNode(camera3DPath) as Camera3D;
        camera3D.SetCullMaskValue(maskNumber, isEnable);
    }

    [Rpc(MultiplayerApi.RpcMode.AnyPeer, CallLocal = true, TransferMode = MultiplayerPeer.TransferModeEnum.Reliable)]
    public void RpcChangeAlbedoOfGeometry(NodePath nodePath, Color color)
    {
        GeometryInstance3D geometry3DInstance = GetNode(nodePath) as GeometryInstance3D;
        StandardMaterial3D standardMaterial3D = geometry3DInstance.MaterialOverride.Duplicate() as StandardMaterial3D;
        geometry3DInstance.MaterialOverride = standardMaterial3D;
        standardMaterial3D.AlbedoColor = color;
    }
}
