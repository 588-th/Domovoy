using Godot;
using Godot.Collections;

public partial class SpawnerItem : MultiplayerSpawner
{
    [Export] private Array<Marker3D> _markers;
    [Export] private Array<PackedScene> _packedScenes;

    public override void _Ready()
    {
        SpawnFunction = new Callable(this, nameof(CustomSpawnFunction));
    }

    public void SpawnItems()
    {
        for (int i = 0; i < _packedScenes.Count; i++)
        {
            Dictionary spawnData = new()
            {
                {"number", i},
                {"itemScene", _packedScenes[i].ResourcePath}
            };
            Spawn(spawnData);
        }
    }

    private Node CustomSpawnFunction(Variant data)
    {
        Dictionary spawnData = (Dictionary)data;

        int number = (int)spawnData["number"];
        string itemScenePath = (string)spawnData["itemScene"];
        PackedScene itemScene = ResourceLoader.Load<PackedScene>(itemScenePath);
        RigidBody3D itemRoot = itemScene.Instantiate<RigidBody3D>();

        itemRoot = SetPosition(itemRoot, number);
        itemRoot = SetNetSettings(itemRoot);

        return itemRoot;
    }

    private RigidBody3D SetPosition(RigidBody3D itemRoot, int number)
    {
        itemRoot.Position = _markers[number].Position;
        itemRoot.Rotation = _markers[number].Rotation;

        return itemRoot;
    }

    private RigidBody3D SetNetSettings(RigidBody3D itemRoot)
    {
        if (Multiplayer.GetUniqueId() != 1)
            itemRoot.GetNode("ServerPart").Free();

        return itemRoot;
    }
}
