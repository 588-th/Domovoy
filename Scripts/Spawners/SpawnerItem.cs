using Godot;
using System.Collections.Generic;

public partial class SpawnerItem : MultiplayerSpawner
{
    [Export] private Marker3D[] _markers;
    [Export] private PackedScene[] _packedScenes;

    [ExportGroup("Random")]
    [Export] private bool _random;
    [Export] private Marker3D[] _markersRandom;
    [Export] private PackedScene[] _packedScenesRandom;

    public override void _Ready()
    {
        SpawnFunction = new Callable(this, nameof(CustomSpawnFunction));
    }

    public void SpawnItems()
    {
        for (int i = 0; i < _packedScenes.Length; i++)
            Spawn(i, _packedScenes[i], false);

        if (!_random || _packedScenesRandom.Length == 0)
            return;

        RandomNumberGenerator rng = new();
        List<int> availableMarkers = new(_markersRandom.Length);
        for (int i = 0; i < _markersRandom.Length; i++)
            availableMarkers.Add(i);

        for (int i = 0; i < _packedScenesRandom.Length && availableMarkers.Count > 0; i++)
        {
            int markerIndex = availableMarkers[rng.RandiRange(0, availableMarkers.Count - 1)];
            Spawn(markerIndex, _packedScenesRandom[i], true);
            availableMarkers.Remove(markerIndex);
        }
    }

    private void Spawn(int markerIndex, PackedScene scene, bool isRandom)
    {
        Spawn(new Godot.Collections.Dictionary
        {
            { "index", markerIndex },
            { "scene", scene.ResourcePath },
            { "random", isRandom }
        });
    }

    private Node CustomSpawnFunction(Variant data)
    {
        Godot.Collections.Dictionary spawnData = (Godot.Collections.Dictionary)data;
        PackedScene itemScene = ResourceLoader.Load<PackedScene>((string)spawnData["scene"]);
        RigidBody3D itemRoot = itemScene.Instantiate<RigidBody3D>();

        int index = (int)spawnData["index"];
        bool isRandom = (bool)spawnData["random"];
        itemRoot.Position = (isRandom ? _markersRandom : _markers)[index].Position;
        itemRoot.Rotation = (isRandom ? _markersRandom : _markers)[index].Rotation;

        if (Multiplayer.GetUniqueId() != 1)
            itemRoot.GetNode("ServerPart").QueueFree();

        return itemRoot;
    }
}
