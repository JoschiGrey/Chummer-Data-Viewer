using System.Xml.Serialization;
using Chummer_Database.Enums;
using Chummer_Database.Interfaces;


namespace Chummer_Database.Classes;
/// <summary>
/// Contains all the deserialized information of an Weapon.
/// Use the cleaned up Weapon instead
/// </summary>
[XmlRoot(ElementName="weapon")]
public class XmlWeapon : ICreatable
{
	/// <summary>
	/// THis list only contains names!
	/// </summary>
	[XmlArray("accessories")]
	[XmlArrayItem("accessory", typeof(Accessory))]
	public List<Accessory> AccessoriesNameList { get; set; } = new();
	
	[XmlElement(ElementName="id")] 
	public Guid Id { get; set; }

	[XmlElement(ElementName="name")] 
	public string Name { get; set; } = string.Empty;

	[XmlElement(ElementName = "category")] 
	public string CategoryAsString { get; set; } = string.Empty;
	

	[XmlElement(ElementName = "type")] 
	public string RangeType { get; set; } = string.Empty;
	
	[XmlElement(ElementName="conceal")] 
	public int Conceal { get; set; }

	[XmlElement(ElementName = "accuracy")] 
	public string AccuracyString { get; set; } = string.Empty;


	[XmlElement(ElementName="reach")] 
	public int Reach { get; set; }
	
	[XmlElement(ElementName = "damage")] 
	public string DamageString { get; set; } = string.Empty;
	

	[XmlElement(ElementName = "ap")] 
	public string ApString { get; set; } = string.Empty;
	

	[XmlElement(ElementName = "mode")] 
	public string Mode { get; set; } = string.Empty;

	[XmlElement(ElementName="rc")] 
	public string RecoilCompensationString { get; set; } = string.Empty;
	

	/// <summary>
	/// Raw deserialized string. Use Ammo.DisplayString instead
	/// </summary>
	[XmlElement(ElementName = "ammo")] 
	public string AmmoStringDeserialized { get; set; } = "-";
	

	[XmlElement(ElementName="avail")] 
	public string AvailabilityString { get; set; } = string.Empty;
	

	[XmlElement(ElementName="cost")] 
	public string CostString { get; set; } = string.Empty;
	

	[XmlElement(ElementName="source")] 
	public string BookCode { get; set; } = string.Empty;

	[XmlElement(ElementName="page")] 
	public int Page { get; set; }
	
	[XmlArray("accessorymounts")]
	[XmlArrayItem("mount", typeof(AccessoryMount))]
	public List<AccessoryMount> AccessoryMounts { get; set; } = new();
	
	[XmlArray(ElementName="underbarrels")]
	[XmlArrayItem("underbarrel")]
	public List<string> UnderBarrelsAsStrings { get; set; } = new();

	[XmlElement(ElementName = "addweapon")]
	public List<string> RelatedWeaponsAsStrings { get; set; } = new();

	/// <summary>
	/// The pure deserialized string use Weapon.Ammo.AmmoCategory instead
	/// </summary>
	[XmlElement(ElementName = "ammocategory")]
	public string? AmmoCategoryDeserialized { get; set; }


	
	
	[XmlElement(ElementName = "spec")]
	private string Specialisation { get; set; } = string.Empty;
	
	[XmlElement(ElementName="spec2")] 
	private string SecondarySpecialisation { get; set; } = string.Empty;
	

	[XmlElement(ElementName = "useskill")] 
	public string UseSkillString { get; set; } = string.Empty;
	

	[XmlElement(ElementName = "range")] 
	public string WeaponRangeString { get; set; } = string.Empty;

	[XmlElement(ElementName = "alternaterange")]
	public string AlternateRangeString { get; set; } = string.Empty;

   [XmlElement(ElementName="ammoslots")] 
   public int AmmoSlots { get; set; } 
   
   [XmlElement(ElementName="cyberware")] 
   public bool IsCyberware { get; set; }
   
   /* Only False values present in XML, seems to be used for display purposes... with the exception of microtorpedo in Micro-Torpedo Launcher
   [XmlElement(ElementName="requireammo")] 
   public string Requireammo { get; set; } */
   
   
   /// <summary>
   /// Used to assign a more specific type to a category / weapon. Use Category.Type instead.
   /// </summary>
   [XmlElement(ElementName="weapontype")] 
   public string WeaponType { get; set; } = string.Empty;
   
   [XmlElement(ElementName="sizecategory")] 
   public string SizeCategory { get; set; } = string.Empty;
   
   [XmlElement(ElementName="allowaccessory")] 
   public bool AllowAccessory { get; set; } 
   
   /// <summary>
   /// Determines the "specialisation" / kind of Exotic Ranged Weapon that has to be used.
   /// </summary>
   [XmlElement(ElementName="useskillspec")] 
   public string UseSkillSpecialisation { get; set; } = string.Empty;
   
   /*
   [XmlElement(ElementName="required")] 
   public Required Required { get; set; } 
   */

	private AccessoryMount _extraMount;
	/// <summary>
	/// Seems to be used by underbarrel weapons to replace the used up mount in chummer. Not really needed here -> just add it to the mounts list.
	/// </summary>
	[XmlElement(ElementName="extramount")] 
	public AccessoryMount ExtraMount
	{
		get => _extraMount;
		set
		{
			_extraMount = value;
			AccessoryMounts.Add(value);
		} 
	} 
	
	/// <summary>
	/// Mostly used by underbarrel weapons to show what mount they use.
	/// </summary>
	[XmlElement(ElementName="mount")] 
	public AccessoryMount Mount { get; set; }

	[XmlArray("allowgear")]
	[XmlArrayItem(ElementName = "gearcategory")]
	public List<string> AllowedGearCategories { get; set; } = new();
	
	/// <summary>
	/// Special Case for Hammerli Gemini, that barrel gear costs double since it has 2 barrels... yeah not needed here I guess
	/// </summary>
	[XmlArray("doubledcostaccessorymounts")]
	[XmlArrayItem(ElementName = "mount")]
	public List<AccessoryMount>? DoubledCostAccessoryMounts { get; set; }

	/// <summary>
	/// Special Case for Hammerli Gemini... it shopots 2 bullets at singleshot
	/// </summary>
	[XmlElement(ElementName = "singleshot")]
	public int SingleShot { get; set; } = 1;

	/// <summary>
	/// Special Case for Hammerli Gemini. It shoots 6 instead of 3 for short bursts.
	/// </summary>
	[XmlElement(ElementName="shortburst")] 
	public int ShortBurst { get; set; } = 3;


	[XmlElement(ElementName = "maxrating")]
	public int MaxRating { get; set; }

	/* This exists, but I'm fairly certain I don't need it. Chummer uses it to hide stuff from lists, but atm I don't wanna do this.
	[XmlElement(ElementName="hide")] 
	public object Hide { get; set; } 
	*/

	

	public Task CreateAsync(ILogger logger, ICreatable? baseObject)
	{
		Weapon newWeapon = new Weapon()
		{
			Id = Id
		};
		
		return newWeapon.CreateAsync(logger, this);
	}
}



