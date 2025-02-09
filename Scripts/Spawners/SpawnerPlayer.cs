using Godot;
using Godot.Collections;

public partial class SpawnerPlayer : MultiplayerSpawner
{
    [Export] private Node _playersParent;
    private int _spawnCount;

    public override void _Ready()
    {
        SpawnFunction = new Callable(this, nameof(CustomSpawnFunction));
    }

    public void SpawnPlayer(long peerID, PackedScene playerScene, Marker3D marker)
    {
        Spawn(new Dictionary
        {
            { "peerID", peerID },
            { "scene", playerScene.ResourcePath },
            { "marker", marker.GetPath() }
        });
    }

    private Node CustomSpawnFunction(Variant data)
    {
        Dictionary spawnData = (Dictionary)data;
        PackedScene playerScene = ResourceLoader.Load<PackedScene>((string)spawnData["scene"]);
        Marker3D marker3D = GetNode(spawnData["marker"].ToString()) as Marker3D;
        Node playerRoot = playerScene.Instantiate();

        long peerID = (long)spawnData["peerID"];
        playerRoot.Name = peerID.ToString();

        SetSpawnPosition(playerRoot, marker3D);
        SetNetSettings(playerRoot, peerID);

        _spawnCount++;
        return playerRoot;
    }

    public void DespawnPlayer(int peerID)
    {
        _playersParent.GetNodeOrNull(peerID.ToString())?.QueueFree();
        _spawnCount--;
    }

    private void SetSpawnPosition(Node playerRoot, Marker3D marker3D)
    {
        CharacterBody3D playerBody = playerRoot.GetNode<CharacterBody3D>("CommonPart/PlayerBody");
        Camera3D playerCamera = playerRoot.GetNode<Camera3D>("CommonPart/CameraHolder/PlayerCamera");

        playerBody.Position = marker3D.Position;
        playerBody.Rotation = marker3D.Rotation;
        playerCamera.Rotation = marker3D.Rotation;
    }

    private void SetNetSettings(Node playerRoot, long peerID)
    {
        int uniqueID = Multiplayer.GetUniqueId();
        int authorityID = (int)peerID;

        SetAuthority(playerRoot, authorityID);
        SetSyncronizersVisibility(playerRoot, authorityID);
        FreeParts(playerRoot, authorityID, uniqueID);
    }

    private void SetAuthority(Node playerRoot, int authorityID)
    {
        SetMultiplayerAuthoritys(playerRoot, "CommonPart", authorityID);
        SetMultiplayerAuthoritys(playerRoot, "ClientPart", authorityID);
        SetMultiplayerAuthoritys(playerRoot, "ClientServerPart", authorityID);
        SetMultiplayerAuthoritys(playerRoot, "CommonPart/PlayerBody", 1);
        SetMultiplayerAuthoritys(playerRoot, "CommonPart/CameraHolder/PlayerCamera/Audio3D", 1);
        SetMultiplayerAuthoritys(playerRoot, "ClientServerPart/Scripts/HUDParameters", 1);
        SetMultiplayerAuthoritys(playerRoot, "ClientServerPart/Scripts/MovementActions", 1);
        SetMultiplayerAuthoritys(playerRoot, "ClientServerPart/Scripts/MeleeAttackActions", 1);
        SetMultiplayerAuthoritys(playerRoot, "ClientServerPart/Audio", 1);
        SetMultiplayerAuthoritys(playerRoot, "Synchronizers/SynchronizerPlayerCamera", authorityID);
        SetMultiplayerAuthoritys(playerRoot, "Synchronizers/SynchronizerInputVector", authorityID);
    }

    private void SetSyncronizersVisibility(Node playerRoot, int authorityID)
    {
        SetVisibility(playerRoot, "Synchronizers/SynchronizerHUDParameters", 1, true);
        SetVisibility(playerRoot, "Synchronizers/SynchronizerHUDParameters", authorityID, true);
        SetVisibility(playerRoot, "Synchronizers/SynchronizerInputVector", 1, true);
        SetVisibility(playerRoot, "Synchronizers/SynchronizerInputVector", authorityID, true);
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
