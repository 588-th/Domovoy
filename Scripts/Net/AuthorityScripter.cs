using Godot;
using Godot.Collections;

public partial class AuthorityScripter : Node
{
    public override void _EnterTree()
    {
        if (!IsMultiplayerAuthority())
            RemoveChildren();
    }

    private void RemoveChildren()
    {
        Array<Node> children = GetChildren();
        foreach (Node child in children)
            child.SetScript((GodotObject)null);
        QueueFree();
    }
}
