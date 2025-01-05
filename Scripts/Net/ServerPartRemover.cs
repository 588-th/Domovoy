using Godot;
using Godot.Collections;

public partial class ServerPartRemover : Node
{
    [Export] private Array<Node> _serverParts;

    public override void _EnterTree()
    {
        if (!Multiplayer.IsServer())
            RemoveServerParts();
    }

    private void RemoveServerParts()
    {
        foreach (Node serverPart in _serverParts)
        {
            serverPart._ExitTree();
            serverPart.QueueFree();
        }
    }
}
