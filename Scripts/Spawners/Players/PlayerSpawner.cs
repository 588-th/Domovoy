using Godot;
using Godot.Collections;

public partial class PlayerSpawner : MultiplayerSpawner
{
    [Export] private Node _playersParent;
    [Export] private Array<Marker3D> _spawnMarkers;
    private int _spawnCount;

    public override void _Ready()
    {
        SpawnFunction = new Callable(this, nameof(CustomSpawnFunction));
    }

    public void SpawnPlayer(long peerID, PackedScene playerScene)
    {
        Dictionary spawnData = new()
        {
            {"peerID", peerID},
            {"playerScene", playerScene.ResourcePath}
        };

        Spawn(spawnData);
    }

    private Node CustomSpawnFunction(Variant data)
    {
        Dictionary spawnData = (Dictionary)data;

        long peerID = (long)spawnData["peerID"];
        string playerScenePath = (string)spawnData["playerScene"];
        PackedScene playerScene = ResourceLoader.Load<PackedScene>(playerScenePath);

        Node playerRoot = playerScene.Instantiate();
        playerRoot.Name = peerID.ToString();

        playerRoot = SetSpawnPosition(playerRoot);
        playerRoot = SetNetSettings(playerRoot, peerID);

        _spawnCount++;
        return playerRoot;
    }

    public void DespawnPlayer(int peerID)
    {
        if (_playersParent.HasNode(peerID.ToString()))
        {
            Node playerNode = _playersParent.GetNode(peerID.ToString());
            playerNode.QueueFree();
            _spawnCount--;
        }
    }

    private Node SetSpawnPosition(Node playerRoot)
    {
        int markerNumber = _spawnCount;

        CharacterBody3D playerbody = playerRoot.GetNode<CharacterBody3D>("CommonPart/PlayerBody");
        playerbody.Position = _spawnMarkers[markerNumber].Position;
        playerbody.Rotation = _spawnMarkers[markerNumber].Rotation;

        Camera3D playerCamera = playerRoot.GetNode<Camera3D>("CommonPart/CameraHolder/PlayerCamera");
        playerCamera.Rotation = _spawnMarkers[markerNumber].Rotation;
        return playerRoot;
    }

    private Node SetNetSettings(Node playerRoot, long peerID)
    {
        int uniqueID = Multiplayer.GetUniqueId();
        int authorityID = (int)peerID;

        SetAuthority(playerRoot, authorityID);
        SetSyncronizersVisibility(playerRoot, authorityID);
        FreeParts(playerRoot, authorityID, uniqueID);

        return playerRoot;
    }

    private void SetAuthority(Node playerRoot, int authorityID)
    {
        SetMultiplayerAuthoritys(playerRoot, "CommonPart", authorityID);
        SetMultiplayerAuthoritys(playerRoot, "ClientPart", authorityID);
        SetMultiplayerAuthoritys(playerRoot, "ClientServerPart", authorityID);
        SetMultiplayerAuthoritys(playerRoot, "CommonPart/PlayerBody", 1);
        SetMultiplayerAuthoritys(playerRoot, "CommonPart/CameraHolder/PlayerCamera/Audio3D", 1);
        SetMultiplayerAuthoritys(playerRoot, "CommonPart/CameraHolder/PlayerCamera/Audio3D/Voice", authorityID);
        SetMultiplayerAuthoritys(playerRoot, "ClientServerPart/Scripts/HUDParameters", 1);
        SetMultiplayerAuthoritys(playerRoot, "ClientServerPart/Scripts/MovementActions", 1);
        SetMultiplayerAuthoritys(playerRoot, "ClientServerPart/Scripts/MeleeAttackActions", 1);
        SetMultiplayerAuthoritys(playerRoot, "ClientServerPart/Audio", 1);
        SetMultiplayerAuthoritys(playerRoot, "Synchronizers/PlayerCameraSynchronizer", authorityID);
        SetMultiplayerAuthoritys(playerRoot, "Synchronizers/InputVectorSynchronizer", authorityID);
    }

    private void SetSyncronizersVisibility(Node playerRoot, int authorityID)
    {
        SetVisibility(playerRoot, "Synchronizers/HUDParametersSynchronizer", 1, true);
        SetVisibility(playerRoot, "Synchronizers/HUDParametersSynchronizer", authorityID, true);
        SetVisibility(playerRoot, "Synchronizers/InputVectorSynchronizer", 1, true);
        SetVisibility(playerRoot, "Synchronizers/InputVectorSynchronizer", authorityID, true);
    }

    private void FreeParts(Node playerRoot, int authorityID, int uniqueID)
    {
        if (uniqueID != authorityID)
            playerRoot.GetNode("ClientPart").Free();
        if (uniqueID != 1)
            playerRoot.GetNode("ServerPart").Free();
        if (uniqueID != authorityID && uniqueID != 1)
            playerRoot.GetNode("ClientServerPart").Free();
    }

    private static void SetMultiplayerAuthoritys(Node playerRoot, string path, int peerID)
    {
        var part = playerRoot.GetNodeOrNull(path);
        part?.SetMultiplayerAuthority(peerID);
    }

    private static void SetVisibility(Node playerRoot, string path, int peerID, bool visible)
    {
        MultiplayerSynchronizer sync = playerRoot.GetNodeOrNull<MultiplayerSynchronizer>(path);
        sync?.SetVisibilityFor(peerID, visible);
    }
}
