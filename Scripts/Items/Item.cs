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

    public virtual void BindActions(InputActions inputHandler)
    {
        
    }

    public virtual void UnbindActions(InputActions inputHandler)
    {

    }

    public virtual string GetItemInfo()
    {
        return "";
    }
}
