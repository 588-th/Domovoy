using Godot;
using Godot.Collections;

public partial class LevelSpawner : Node
{
    [Export] private Node _levelParent;

    public void SpawnLevel(PackedScene levelScene)
    {
        Node levelNode = levelScene.Instantiate();
        _levelParent.AddChild(levelNode, true);
    }

    public void DespawnLevel()
    {
        Array<Node> children = _levelParent.GetChildren();
        foreach (Node child in children)
            child.QueueFree();
    }
}
