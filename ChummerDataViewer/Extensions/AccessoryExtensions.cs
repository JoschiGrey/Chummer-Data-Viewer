using ChummerDataViewer.Classes;
using ChummerDataViewer.Enums;
using ChummerDataViewer.Interfaces;

namespace ChummerDataViewer.Extensions;

public static class AccessoryExtensions
{
    /// <summary>
    /// Tries to get a list of all MountSlots that this Accessory would fit into.
    /// </summary>
    /// <param name="accessory"></param>
    /// <param name="holder"></param>
    /// <param name="logger"></param>
    /// <param name="fittingSlots"></param>
    /// <returns></returns>
    public static bool TryGetFittingSlots(this Accessory accessory, IHoldAccessories holder, ILogger logger, out List<MountSlot> fittingSlots)
    {
        fittingSlots = new List<MountSlot>();
        
        fittingSlots = holder.MountSlots.Where(slot => accessory.Mounts.Contains(slot.Mount)).ToList();
        if (!fittingSlots.Any())
        {
            if(holder is Weapon weapon)
                logger.LogDebug("{Weapon.Name} did not contain a fitting slot for {Accessory.Name}", weapon.Name, accessory.Name);
            else
            {
                logger.LogDebug("{Holder} did not contain a fitting slot for {Accessory.Name}", holder, accessory.Name);
            }
            return false;
        }

        return true;
    }

    public static bool TryGetFreeSlots(this Accessory accessory, IHoldAccessories holder, ILogger logger, out List<MountSlot> freeFittingSlots)
    {
        freeFittingSlots = new List<MountSlot>();
        
        if (!accessory.TryGetFittingSlots(holder, logger, out var fittingSlots))
            return false;

        freeFittingSlots = fittingSlots.Where(slot => slot.Accessory is null).ToList();

        if (!freeFittingSlots.Any())
        {
            if(holder is Weapon weapon)
                logger.LogDebug("{Weapon.Name} did not contain a fitting slot for {Accessory.Name}", weapon.Name, accessory.Name);
            else
            {
                logger.LogDebug("{Holder} did not contain a fitting slot for {Accessory.Name}", holder, accessory.Name);
            }
            return false;
        }

        return true;
    }
}