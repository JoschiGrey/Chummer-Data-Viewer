using ChummerDataViewer.Classes;

namespace ChummerDataViewer.Interfaces;

public interface IHoldAccessories
{
    public HashSet<MountSlot> MountSlots { get; set; }
    
    public bool TryAddAccessory(Accessory accessory, ILogger logger, bool forceAdd = false);
    
    public bool TryAddMultipleAccessories(IEnumerable<Accessory> accessories, ILogger logger, bool forceAdd = false);
    
    public void ForceAddAccessory(Accessory accessory, ILogger logger);
    
    public bool TryRemoveAccessory(Accessory accessory, ILogger logger);


    

    public void AddAccessory(Accessory accessory, ref MountSlot mountSlot, ILogger logger);
    public void RemoveAccessory(Accessory accessory, MountSlot mountSlot, ILogger logger);
    
    
}