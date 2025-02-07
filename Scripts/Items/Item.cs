using Godot;
using System;

public partial class Item : RigidBody3D
{
    [Export] public HotbarSlotType SlotType;
    [Export] public Sprite2D Icon;
    [Export] public AudioStreamMP3 PickupAudio;
    [Export] public AudioStreamMP3 TakeOnHandAudio;
    public Node HoldingPlayer;

    public Action ItemUsed;
    public Action<Item> ItemFree;

    public virtual void BindOnHandActions(InputActions inputAction)
    {
        
    }

    public virtual void UnbindOnHandActions(InputActions inputAction)
    {

    }

    public virtual void BindEquipActions(InputActions inputAction)
    {

    }

    public virtual void UnbindEquipActions(InputActions inputAction)
    {

    }

    public virtual string GetItemInfo()
    {
        return "";
    }

    protected void FreeItem()
    {
        ItemFree?.Invoke(this);
        GetParent().RemoveChild(this);
        QueueFree();
    }
}
