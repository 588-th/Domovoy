using Godot;
using Godot.Collections;

public partial class SpawnerSpectator : MultiplayerSpawner
{
    [Export] private Node _spectatorParent;

    public override void _Ready()
    {
        SpawnFunction = new Callable(this, nameof(CustomSpawnFunction));
    }

    public void SpawnSpectator(long peerID, PackedScene playerScene)
    {
        Dictionary spawnData = new()
        {
            {"peerID", peerID},
            {"spectatorScene", playerScene.ResourcePath},
        };

        Spawn(spawnData);
    }

    private Node CustomSpawnFunction(Variant data)
    {
        Dictionary spawnData = (Dictionary)data;

        long spectatorID = (long)spawnData["peerID"];
        string spectatorScenePath = (string)spawnData["spectatorScene"];
        PackedScene spectatorScene = ResourceLoader.Load<PackedScene>(spectatorScenePath);

        Node spectatorRoot = spectatorScene.Instantiate();
        spectatorRoot.Name = spectatorID.ToString();

        spectatorRoot = SetAuthority(spectatorRoot, spectatorID);

        return spectatorRoot;
    }

    public void DespawnSpectator(int peerID)
    {
        if (_spectatorParent.HasNode(peerID.ToString()))
        {
            Node spectatorNode = _spectatorParent.GetNode(peerID.ToString());
            spectatorNode.QueueFree();
        }
    }

    private Node SetAuthority(Node spectatorRoot, long peerID)
    {
        int uniqueID = Multiplayer.GetUniqueId();
        int authorityID = (int)peerID;

        SetAuthority(spectatorRoot, authorityID);
        SetSyncronizersVisibility(spectatorRoot, authorityID);
        FreeParts(spectatorRoot, authorityID, uniqueID);

        return spectatorRoot;
    }

    private void SetAuthority(Node spectatorRoot, int authorityID)
    {
        SetMultiplayerAuthoritys(spectatorRoot, "CommonPart", authorityID);
        SetMultiplayerAuthoritys(spectatorRoot, "ClientPart", authorityID);
        SetMultiplayerAuthoritys(spectatorRoot, "ClientServerPart", authorityID);
        SetMultiplayerAuthoritys(spectatorRoot, "ClientServerPart/SpectatorCamera", 1);
    }

    private void SetSyncronizersVisibility(Node spectatorRoot, int authorityID)
    {
        SetVisibility(spectatorRoot, "Synchronizers/SynchronizerSpectatorCamera", 1, true);
        SetVisibility(spectatorRoot, "Synchronizers/SynchronizerSpectatorCamera", authorityID, true);
        SetVisibility(spectatorRoot, "Synchronizers/SynchronizerHUDParameters", 1, true);
        SetVisibility(spectatorRoot, "Synchronizers/SynchronizerHUDParameters", authorityID, true);
    }

    private void FreeParts(Node spectatorRoot, int authorityID, int uniqueID)
    {
        if (uniqueID != authorityID)
            spectatorRoot.GetNode("ClientPart").Free();
        if (uniqueID != 1)
            spectatorRoot.GetNode("ServerPart").Free();
        if (uniqueID != authorityID && uniqueID != 1)
            spectatorRoot.GetNode("ClientServerPart").Free();
    }

    private static void SetMultiplayerAuthoritys(Node playerRoot, string path, int authorityID)
    {
        var part = playerRoot.GetNodeOrNull(path);
        part?.SetMultiplayerAuthority(authorityID);
    }

    private static void SetVisibility(Node playerRoot, string path, int peerID, bool visible)
    {
        MultiplayerSynchronizer sync = playerRoot.GetNodeOrNull<MultiplayerSynchronizer>(path);
        sync?.SetVisibilityFor(peerID, visible);
    }
}
