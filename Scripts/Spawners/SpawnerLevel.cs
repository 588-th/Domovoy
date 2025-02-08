using Godot;
using Godot.Collections;

public partial class SpawnerLevel : Node
{
    [Export] private Node _holderLevel;

    public void SpawnLevel(PackedScene levelScene)
    {
        Node levelNode = levelScene.Instantiate();
        _holderLevel.AddChild(levelNode, true);
    }

    public void DespawnLevel()
    {
        Array<Node> levels = _holderLevel.GetChildren();
        foreach (Node level in levels)
            level.QueueFree();
    }
}
