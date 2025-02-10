using Godot;

public partial class DestroyTimer : Timer
{
    [Export] private Node _destroyNode;

    public override void _Ready()
    {
        Timeout += DestroyNode;
    }

    private void DestroyNode()
    {
        Timeout -= DestroyNode;
        _destroyNode.QueueFree();
    }
}
