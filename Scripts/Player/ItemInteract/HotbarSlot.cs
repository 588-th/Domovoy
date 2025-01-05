using Godot;

public partial class HotbarSlot : Node
{
    public HotbarSlotType SlotType;
    public Item Item;

    public HotbarSlot(HotbarSlotType SlotType)
    {
        this.SlotType = SlotType;
    }

    public bool IsEmpty()
    {
        return Item == null;
    }
}
