using ChummerDataViewer.Backend.Enums;

namespace ChummerDataViewer.Backend.Classes;

public record MountSlot
{
    public WeaponMountSlots MountSlots { get; }
    public Accessory? Accessory { get; set; }

    /// <summary>
    /// True if this slot was created especially to add this Accessory
    /// </summary>
    public bool IsForced { get; init; } = false;
    
    
    public MountSlot(WeaponMountSlots mountSlots)
    {
        MountSlots = mountSlots;
    }

    public MountSlot(WeaponMountSlots mountSlots, Accessory accessory)
    {
        MountSlots = mountSlots;
        Accessory = accessory;
    }
}