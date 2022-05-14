using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Blazorise.Extensions;
using Chummer_Database.Enums;


namespace Chummer_Database.Classes;
[XmlRoot(ElementName="weapon")]
public class Weapon
{
	[XmlElement(ElementName="id")] 
	public Guid Id { get; set; }

	[XmlElement(ElementName="name")] 
	public string Name { get; set; } = string.Empty;

	[XmlElement(ElementName = "category")] 
	public string CategoryAsString { get; set; } = string.Empty;

	[XmlIgnore]
	public Category? Category { get; set; }

	[XmlElement(ElementName = "type")] 
	public string RangeType { get; set; } = string.Empty;
	
	[XmlElement(ElementName="conceal")] 
	public int Conceal { get; set; }

	[XmlElement(ElementName = "accuracy")] public string Accuracy { get; set; } = string.Empty;

	[XmlElement(ElementName="reach")] 
	public int Reach { get; set; }
	
	[XmlElement(ElementName = "damage")] 
	public string DamageString { get; set; } = string.Empty;
	
	[XmlIgnore]
	public Damage? Damage { get; set; }

	[XmlElement(ElementName = "ap")] 
	public string Ap { get; set; } = string.Empty;

	[XmlElement(ElementName = "mode")] 
	public string Mode { get; set; } = string.Empty;

	[XmlElement(ElementName="rc")] 
	public string RecoilCompensation { get; set; } = string.Empty;

	/// <summary>
	/// Raw deserialized string. Use Ammo.DisplayString instead
	/// </summary>
	[XmlElement(ElementName = "ammo")] 
	public string AmmoStringDeserialized { get; set; } = "-";
	
	[XmlIgnore]
	public Ammo? Ammo { get; private set; }

	[XmlElement(ElementName="avail")] public string AvailabilityString { get; set; } = string.Empty;
	
	[XmlIgnore]
	public Availability? Availability { get; set; } 

	[XmlElement(ElementName="cost")] public string CostString { get; set; } = string.Empty;
	
	[XmlIgnore]
	public int Cost { get; private set; }

	[XmlElement(ElementName="source")] public string Source { get; set; } = string.Empty;

	[XmlElement(ElementName="page")] public int Page { get; set; }

    [XmlIgnore] 
    public string DisplaySource => $"p.{Page} ({Source})";

    
    [XmlArray("accessories")]
    [XmlArrayItem("accessory")]
    private List<string> AccessoriesStringList { get; set; } = new();

    [XmlIgnore] 
    public List<Accessory> Accessories { get; set; } = new();


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

	[XmlElement(ElementName = "range")] public string RangeCategory { get; set; } = string.Empty;
	
	//TODO: This can be problematic if I ever introduce custom Data
	[XmlIgnore]
	public Range? Range => XmlLoader.RangesXmlData?.RangeDictionary[RangeCategory];
	
	[XmlElement(ElementName = "spec")]
	private string Specialisation { get; set; } = string.Empty;
	
	[XmlElement(ElementName="spec2")] 
	private string SecondarySpecialisation { get; set; } = string.Empty;

	[XmlIgnore] 
	public List<string> Specialisations => new() {Specialisation, SecondarySpecialisation};

	[XmlElement(ElementName = "useskill")] public string UseSkillString { get; set; } = string.Empty;
	
	[XmlIgnore]
	public Skill? Skill { get; set; }


	[XmlElement(ElementName = "alternaterange")]
	public string AlternateRangeString { get; set; } = string.Empty;
   
   [XmlIgnore]
   public Range AlternateRange
   {
	   get
	   {
		   if (XmlLoader.RangesXmlData is null)
			   throw new ArgumentNullException(nameof(XmlLoader.RangesXmlData), "RangesXmlData seems to be missing");

		   var rangeDic = XmlLoader.RangesXmlData.RangeDictionary;
		   
		   if(rangeDic.ContainsKey(AlternateRangeString))
				return rangeDic[AlternateRangeString];

		   foreach (var (key,value) in rangeDic)
		   {
			   if (key.Contains(AlternateRangeString))
				   return value;
		   }

		   return new Range() {RangeCategory = "Undefined Range"};
	   }
   }

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
   private string WeaponType { get; set; } = string.Empty;
   
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


	
	public bool Create(ILogger logger)
	{
		const string numberPattern = "[0-9]+";
		Damage = new Damage(DamageString, logger);
		
        Category = TryGetCategory();

		Skill = TryGetSkill();

		Availability = new Availability(AvailabilityString, logger);

		Ammo = new Ammo(this, logger);

		foreach (var accessoryString in AccessoriesStringList)
		{
			if(WeaponsXmlRoot.AccessoriesDictionary.ContainsKey(accessoryString))
				Accessories.Add(WeaponsXmlRoot.AccessoriesDictionary[accessoryString]);
		}

		try
		{

			var match = Regex.Match(CostString, numberPattern).ToString();
			if (!match.IsNullOrEmpty())
				Cost = int.Parse(match);

		}
		catch (FormatException e)
		{
			logger.LogWarning(e, "Failed to parse {CostString} to an cost", CostString);
		}
		
		return true;

		Category TryGetCategory()
		{
			var catAsString = CategoryAsString;
            Category outCategory;

			if (WeaponsXmlRoot.WeaponCategoryDictionary is null)
				throw new ArgumentNullException(nameof(WeaponsXmlRoot.WeaponCategoryDictionary));

			if (WeaponsXmlRoot.WeaponCategoryDictionary.ContainsKey(catAsString))
			{
				outCategory = WeaponsXmlRoot.WeaponCategoryDictionary[catAsString];
				//If WeaponType is not string.empty it should replace any category defined type.
				if (!WeaponType.IsNullOrEmpty())
					outCategory.Type = WeaponType;
				//If WeaponType is still string.empty, try to set it from the auto detected category
				if (WeaponType.IsNullOrEmpty())
					WeaponType = outCategory.Type;

				return outCategory;
			}

			outCategory = new Category()
			{
				Name = catAsString,
				Type = WeaponType
			};

            if (WeaponsXmlRoot.WeaponCategoryDictionary.ContainsKey(catAsString)) return outCategory;

            WeaponsXmlRoot.WeaponCategoryDictionary.Add(outCategory.Name, outCategory);
            return outCategory;

        }

		Skill TryGetSkill()
		{
			var skillDic = XmlLoader.SkillsXmlData?.SkillsDictionary;

			if (skillDic is null)
				throw new ArgumentNullException(nameof(skillDic),"SkillDictionary is not formed");
			
			//If a useskill is defined it overrides everything
			if(skillDic.ContainsKey(UseSkillString))
				return skillDic[UseSkillString];
			
			//Try to catch the cases in which skill == weapon category
			if (skillDic.ContainsKey(CategoryAsString))
				return skillDic[CategoryAsString];

			//Try to detect skill by Category
			switch (CategoryAsString)
			{
				case "Unarmed" or "Bio-Weapon" or "Cyberweapon":
					return skillDic["Unarmed Combat"];
				case "Bows" or "Crossbows":
					return skillDic["Archery"];
				case "Tasers" or "Light Pistols" or "Heavy Pistols":
					return skillDic["Pistols"];
				case "Machine Pistols" or "Submachine Guns" or "Assault Rifles" or "Carbines" or "Light Machine Guns" or "Medium Machine Guns" or "Heavy Machine Guns":
					return skillDic["Automatics"];
				case "Sporting Rifles" or "Sniper Rifles" or "Shotguns":
					return skillDic["Longarms"];
				case "Assault Cannons" or "Grenade Launchers" or "Missile Launchers":
					return skillDic["Heavy Weapons"];
				default:
					logger.LogDebug("Could not parse useskill for {Category.Name} of {Name}",CategoryAsString, Name);
					return new Skill() {Name = "Undefined Skill"};
				
			}
		}
	}
}


/*
[XmlRoot(ElementName="usegear")]
public class Usegear { 

	[XmlElement(ElementName="name")] 
	public Name Name { get; set; } 

	[XmlElement(ElementName="category")] 
	public string Category { get; set; } 

	[XmlElement(ElementName="rating")] 
	public int Rating { get; set; } 
}

[XmlRoot(ElementName="gears")]
public class Gears { 

	[XmlElement(ElementName="usegear")] 
	public List<Usegear> Usegear { get; set; } 
}

[XmlRoot(ElementName="useskill")]
public class Useskill {
	[XmlAttribute(AttributeName="operation")] 
	public string Operation { get; set; } 
}

[XmlRoot(ElementName="OR")]
public class OR { 

	[XmlElement(ElementName="category")] 
	public List<string> Category { get; set; } 

	[XmlElement(ElementName="ammo")] 
	public List<Ammo> Ammo { get; set; } 

	[XmlElement(ElementName="useskill")] 
	public List<string> Useskill { get; set; } 

	[XmlElement(ElementName="AND")] 
	public AND AND { get; set; } 
}

[XmlRoot(ElementName="AND")]
public class AND { 

	[XmlElement(ElementName="useskill")] 
	public Useskill Useskill { get; set; } 

	[XmlElement(ElementName="OR")] 
	public OR OR { get; set; } 
}

[XmlRoot(ElementName="required")]
public class Required { 

	[XmlElement(ElementName="OR")] 
	public OR OR { get; set; } 

	[XmlElement(ElementName="weapondetails")] 
	public WeaponDetails WeaponDetails { get; set; } 

	[XmlElement(ElementName="oneof")] 
	public Oneof Oneof { get; set; } 
}

[XmlRoot(ElementName="conceal")]
public class Conceal { 

	[XmlAttribute(AttributeName="operation")] 
	public string Operation { get; set; } 

	[XmlText] 
	public int Text { get; set; } 
}

[XmlRoot(ElementName="weapondetails")]
public class WeaponDetails { 

	[XmlElement(ElementName="OR")] 
	public List<OR> OR { get; set; } 

	[XmlElement(ElementName="conceal")] 
	public Conceal Conceal { get; set; } 

	[XmlElement(ElementName="type")] 
	public string Type { get; set; } 
}

[XmlRoot(ElementName="allowgear")]
public class Allowgear { 

	[XmlElement(ElementName="gearcategory")] 
	public List<string> Gearcategory { get; set; } 
}

[XmlRoot(ElementName="doubledcostaccessorymounts")]
public class Doubledcostaccessorymounts { 

	[XmlElement(ElementName="mount")] 
	public string Mount { get; set; } 
}

[XmlRoot(ElementName="name")]
public class Name { 

	[XmlAttribute(AttributeName="select")] 
	public string Select { get; set; } 

	[XmlText] 
	public string Text { get; set; } 

	[XmlAttribute(AttributeName="operation")] 
	public string Operation { get; set; } 
}

[XmlRoot(ElementName="oneof")]
public class Oneof { 

	[XmlElement(ElementName="accessory")] 
	public List<string> Accessory { get; set; } 

	[XmlElement(ElementName="weapondetails")] 
	public WeaponDetails WeaponDetails { get; set; } 

	[XmlElement(ElementName="specialmodificationlimit")] 
	public int Specialmodificationlimit { get; set; } 
}

[XmlRoot(ElementName="forbidden")]
public class Forbidden { 

	[XmlElement(ElementName="oneof")] 
	public Oneof Oneof { get; set; } 

	[XmlElement(ElementName="weapondetails")] 
	public WeaponDetails WeaponDetails { get; set; } 
}

[XmlRoot(ElementName="ammo")]
public class Ammo { 

	[XmlAttribute(AttributeName="operation")] 
	public string Operation { get; set; } 

	[XmlText] 
	public string Text { get; set; } 
}

[XmlRoot(ElementName="addunderbarrels")]
public class Addunderbarrels { 

	[XmlElement(ElementName="weapon")] 
	public string Weapon { get; set; } 
}

[XmlRoot(ElementName="damage")]
public class DamageOperation { 

	[XmlAttribute(AttributeName="operation")] 
	public string Operation { get; set; } 

	[XmlText] 
	public string Text { get; set; } 
}
*/



