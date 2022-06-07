using ChummerDataViewer.Classes;

namespace ChummerDataViewer.Interfaces;

public interface IHoldAccessories
{
    public ValueTask AddAccessory(Accessory accessory, MountSlot mountSlot, ILogger logger);

    public void RemoveAccessory(Accessory accessory, MountSlot mountSlot, ILogger logger);

    public HashSet<MountSlot> MountSlots { get; set; }
}