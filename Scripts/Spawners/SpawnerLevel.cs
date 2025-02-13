using Godot;
using Godot.Collections;

public partial class SpawnerLevel : MultiplayerSpawner
{
    [Export] private Node _holderLevel;

    public override void _Ready()
    {
        SpawnFunction = new Callable(this, nameof(CustomSpawnFunction));
    }

    public void SpawnLevel(PackedScene levelScene)
    {
        Spawn(new Dictionary
        {
            { "scene", levelScene.ResourcePath},
        });
    }

    private Node CustomSpawnFunction(Variant data)
    {
        Dictionary spawnData = (Dictionary)data;
        PackedScene levelScene = ResourceLoader.Load<PackedScene>((string)spawnData["scene"]);
        Node levelRoot = levelScene.Instantiate();

        long peerID = Multiplayer.GetUniqueId();
        SetNetSettings(levelRoot, peerID);

        return levelRoot;
    }

    public void DespawnLevel()
    {
        Array<Node> levels = _holderLevel.GetChildren();
        foreach (Node level in levels)
            level.QueueFree();
    }

    private void SetNetSettings(Node levelRoot, long peerID)
    {
        int uniqueID = Multiplayer.GetUniqueId();
        int authorityID = (int)peerID;

        FreeParts(levelRoot, authorityID, uniqueID);
    }

    private void FreeParts(Node levelRoot, int authorityID, int uniqueID)
    {
        if (uniqueID != authorityID)
            levelRoot.GetNode("ClientPart")?.Free();
        if (uniqueID != 1)
            levelRoot.GetNode("ServerPart")?.Free();
        if (uniqueID != authorityID && uniqueID != 1)
            levelRoot.GetNode("ClientServerPart")?.Free();
    }
}
