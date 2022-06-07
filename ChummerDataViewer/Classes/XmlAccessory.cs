using System.Xml.Serialization;
using ChummerDataViewer.Interfaces;

namespace ChummerDataViewer.Classes;

[XmlRoot(ElementName="accessory")]
public class XmlAccessory : ICreatable
{
    [XmlElement(ElementName="id")] 
    public Guid Id { get; set; }
    
    [XmlElement(ElementName = "name")] 
    public string Name { get; set; } = string.Empty;
    
    [XmlElement(ElementName = "avail")] 
    public string Avail { get; set; } = string.Empty;
    
    [XmlElement(ElementName = "cost")] 
    public string Cost { get; set; } = string.Empty;
    
    [XmlElement(ElementName="source")] 
    public string BookCode { get; set; } = string.Empty;
    
    [XmlElement(ElementName="page")] 
    public int Page { get; set; }
    
    [XmlElement(ElementName = "mount")]
    public string MountString { get; set; } = string.Empty;
    
    [XmlElement(ElementName="rating")] 
    public int Rating { get; set; } 
    
    //Recoil Comp Modifications
    [XmlElement(ElementName="rc")] 
    public int Rc { get; set; } 
    
    [XmlElement(ElementName="rcdeployable")] 
    public bool Rcdeployable { get; set; } 
    
    [XmlElement(ElementName="rcgroup")] 
    public int RcGroup { get; set; }

    [XmlElement("conceal")] 
    public string Concealment { get; set; } = string.Empty;
    
    [XmlElement("accuracy")]
    public int Accuracy { get; set; }
    
    //TODO: Parse this into an Ammo Object
    [XmlElement("ammoreplace")] 
    public string AmmoReplace { get; set; } = string.Empty;
    
    //Only exists on the Improved Range finder yay special case handling
    [XmlElement("rangemodifier")]
    public int RangeModifier { get; set; }

    [XmlElement("accessorycostmultiplier")]
    public int AccessoryCostMultiplier { get; set; } = 1;

    //Merge this into the ammo object and create an Ammo + Ammo operation
    [XmlElement("ammobonus")] 
    public string AmmoBonus { get; set; } = string.Empty;

    [XmlElement("extramount")] public 
    string ExtraMount { get; set; } = string.Empty;

    [XmlArray("addunderbarrels")]
    [XmlArrayItem("weapon", typeof(string))]
    public List<string> UnderBarrelWeapons { get; set; } = new();
    
    [XmlElement("damage")]
    public int DamageAmountModification { get; set; }
    
    [XmlElement("damagetype")] 
    public string DamageTypeOverride { get; set; } = string.Empty;

    //This feeds Calculations like +(Weapon * 0.5). Maybe we can an Action out of this?
    [XmlElement("modifyammocapacity")] 
    public string ModifyAmmoCapacity { get; set; } = string.Empty;
    
    [XmlElement("ap")]
    public int ArmorPenModification { get; set; }
    
    //I believe this is used for the special modifications quality. I don't think we actually need it.
    [XmlElement("specialmodification")]
    public bool IsSpecialModification { get; set; }
    
    [XmlElement("reach")]
    public int ReachModifier { get; set; }

    public async Task CreateAsync(ILogger logger, ICreatable? baseObject = null)
    {
        var newAccessory = new Accessory();
        await newAccessory.CreateAsync(logger, this);
    }
}