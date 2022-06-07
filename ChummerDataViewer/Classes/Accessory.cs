using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Blazorise.Extensions;
using ChummerDataViewer.Classes.HelperMethods;
using ChummerDataViewer.Extensions;
using ChummerDataViewer.Enums;
using ChummerDataViewer.Interfaces;


namespace ChummerDataViewer.Classes;


public record Accessory : ICreatable, IHasSource
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    
    public Availability? Availability { get; set; }
    
    public int Cost { get; set; }
    
    public int Rating { get; set; } 
    
    //Recoil Comp Modifications
    public int RecoilModification { get; set; } 
    
    public bool RcDeployable { get; set; } 
    
    public int RcGroup { get; set; }
    
    public int ConcealModification { get; set; }
    
    public int Accuracy { get; set; }

    public Ammo? AmmoModification { get; set; }
    
    //This feeds Calculations like +(Weapon * 0.5). Maybe we can an Action out of this?
    public string ModifyAmmoCapacity { get; set; } = string.Empty;
    
    //Only exists on the Improved Range finder yay special case handling
    //public int RangeModifier { get; set; }
    
    public int AccessoryCostMultiplier { get; set; } = 1;

    public List<WeaponMountSlots> ExtraMount { get; set; } = new();
    
    //This sadly can't be made into a List<Weapon> because Weapon is not created at this point and this cannot depend on Weapon, because weapon depends on Accessory.
    //TODO: Just like in weapons delay this by hooking it up to an observable collection of weapons
    public List<string> UnderBarrelWeaponNames { get; set; } = new();
    
    public int DamageAmountModification { get; set; }
    
    //TODO: Parse this into the Damage Type
    public string DamageTypeOverride { get; set; } = string.Empty;
    
    public int ArmorPenModification { get; set; }
    
    public int ReachModifier { get; set; }
    
    public HashSet<WeaponMountSlots> PossibleMountSlots { get; set; } = new();
    public Source? Source { get; set; }
    
    public static Dictionary<string, Accessory> AccessoriesDictionary { get; } = new();
    
    

    public async Task CreateAsync(ILogger logger, ICreatable? baseObject = null)
    {
        if(baseObject is not XmlAccessory baseAccessory)
            return;

        Cost = await Task.Run(() => RegexHelper.GetInt(baseAccessory.Cost, logger)).ConfigureAwait(false);
        ExtraMount = await  Task.Run(()=>GetMounts(baseAccessory.ExtraMount)).ConfigureAwait(false);
        PossibleMountSlots = await  Task.Run(()=>GetMounts(baseAccessory.MountString).ToHashSet()).ConfigureAwait(false);
        ConcealModification = await Task.Run(()=>GetConcealValue(baseAccessory.Concealment)).ConfigureAwait(false);
        Id = baseAccessory.Id;
        Name = baseAccessory.Name;
        Availability = new Availability(baseAccessory.Avail, logger);
        Rating = baseAccessory.Rating;
        RecoilModification = baseAccessory.Rc;
        RcDeployable = baseAccessory.Rcdeployable;
        RcGroup = baseAccessory.RcGroup;
        ModifyAmmoCapacity = baseAccessory.ModifyAmmoCapacity;
        //RangeModifier = baseAccessory.RangeModifier;
        AccessoryCostMultiplier = baseAccessory.AccessoryCostMultiplier;
        Accuracy = baseAccessory.Accuracy;
        AmmoModification = new Ammo(baseAccessory, logger);
        UnderBarrelWeaponNames = baseAccessory.UnderBarrelWeapons;
        DamageAmountModification = baseAccessory.DamageAmountModification;
        DamageTypeOverride = baseAccessory.DamageTypeOverride;
        ArmorPenModification = baseAccessory.ArmorPenModification;
        ReachModifier = baseAccessory.RangeModifier;
        Source = new Source(baseAccessory.BookCode, baseAccessory.Page, logger);




        if (!AccessoriesDictionary.ContainsKey(Name))
            AccessoriesDictionary.Add(Name, this);
        
        int GetConcealValue(string baseConceal)
        {
            if (baseConceal.IsNullOrEmpty())
                return 0;
            
            if (baseConceal.Equals("Rating"))
                return baseAccessory.Rating;
            
            try
            {
                return int.Parse(baseConceal);
            }
            catch (FormatException e)
            {
                logger.LogWarning("Unrecognised Conceal Value pattern {BaseConceal}", baseConceal);
                throw;
            }
        }

        const string accessoryMountStringPattern = @"([A-Z])\w+";
        List<WeaponMountSlots> GetMounts(string? mountString)
        {

            
            var returnList = new List<WeaponMountSlots>();

            if (mountString.IsNullOrEmpty())
                return returnList;
            
            var matches = Regex.Matches(mountString!, accessoryMountStringPattern);
            foreach (Match match in matches)
            {
                if(Enum.TryParse<WeaponMountSlots>(match.ToString(), out var accessoryMount))
                    returnList.Add(accessoryMount);
            }

            return returnList;
        }
    }
    
    

    public static Accessory GetAccessory(string name, ILogger logger)
    {
        return AccessoriesDictionary.GetValueByString(name, logger);
    }

    public static Accessory GetAccessory(Accessory accessoryWithName, ILogger logger)
    {
        return GetAccessory(accessoryWithName.Name, logger);
    }


}
