using ChummerDataViewer.Enums;

namespace ChummerDataViewer.Classes;

public record MountSlot
{
    public AccessoryMount Mount { get; }
    public Accessory? Accessory { get; set; }

    /// <summary>
    /// True if this slot was created especially to add this Accessory
    /// </summary>
    public bool IsForced { get; init; } = false;
    
    
    public MountSlot(AccessoryMount mount)
    {
        Mount = mount;
    }

    public MountSlot(AccessoryMount mount, Accessory accessory)
    {
        Mount = mount;
        Accessory = accessory;
    }
}