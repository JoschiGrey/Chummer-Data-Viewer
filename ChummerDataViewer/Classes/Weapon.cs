using System.Text.RegularExpressions;
using Blazorise.Extensions;
using ChummerDataViewer.Extensions;
using ChummerDataViewer.Interfaces;

namespace ChummerDataViewer.Classes;

public record Weapon : ICreatable
{
    public Guid Id { get; init; }
    
    public string Name { get; private set; } = string.Empty;
    
    public Category? Category { get; private set; }
    
    public int Accuracy { get; private set; }
    
    public Damage? Damage { get; private set; }
    
    public int Ap { get; private set; }
    
    public int RecoilCompensation { get; private set; }
    
    public Ammo? Ammo { get; private set; }
    
    public Availability? Availability { get; private set; } 
    
    public int Cost { get; private set; }
    
    public Source? Source { get; private set; }
    
    public WeaponRange? Range { get; private set; }
    
    public WeaponRange? AlternateWeaponRange { get; private set; }
    
    public string RangeType { get; private set; } = string.Empty;
    public Skill? Skill { get; private set; }
    
    public int Conceal { get; private set; }
    
    //TODO:This should be reworked into it's own class
    public string FiringMode { get; private set; } = string.Empty;


    //Melee Exclusive
    public int Reach { get; private set; }

    public List<Accessory> Accessories { get; private set; } = new();
    
    private const string NumberPattern = "[0-9]+";

    public string CostCss { get; private set; } = string.Empty;

    public static HashSet<Weapon> AllWeapons { get; } = new();
    

    public async Task CreateAsync(ILogger logger, ICreatable? baseObject)
    {
	    if (baseObject is not XmlWeapon baseWeapon)
		    throw new ArgumentException(nameof(baseObject));
	    try
	    {
		    Name = baseWeapon.Name;
		    Damage = new Damage(baseWeapon.DamageString, logger);
		    Availability = new Availability(baseWeapon.AvailabilityString, logger);
		    Ammo = new Ammo(baseWeapon, logger);
		    Source = new Source(baseWeapon.BookCode, baseWeapon.Page, logger);

		    RangeType = baseWeapon.RangeType;
		    Conceal = baseWeapon.Conceal;
		    Reach = baseWeapon.Reach;
		    FiringMode = baseWeapon.Mode;


		    Ap = await GetInt(baseWeapon.ApString);

		    Accuracy = await GetInt(baseWeapon.AccuracyString);

		    RecoilCompensation = await GetInt(baseWeapon.RecoilCompensationString);

		    Category = await GetCategory();
		    Range = WeaponRange.GetWeaponRange(baseWeapon.WeaponRangeString, Category.Name);
		    AlternateWeaponRange = WeaponRange.GetWeaponRange(baseWeapon.AlternateRangeString, Category.Name);

		    Skill = await GetSkill();

		    Cost = await GetInt(baseWeapon.CostString);

		    Accessories = await GetAccessories();

		    CostCss = baseWeapon.FormCostCssClass();
		    
		    AllWeapons.Add(this);
	    }
	    catch (Exception e)
	    {
		    logger.LogCritical(e, "Creation failed of {Type}, {Name}", GetType(), Name);
		    throw;
	    }



	    async ValueTask<int> GetInt(string input)
	    {
		    return await Task.Run(() =>
		    {
			    int output = 0;
			    try
			    {
				    var match = Regex.Match(input, NumberPattern).ToString();
				    if (!match.IsNullOrEmpty())
					    output = int.Parse(match);
			    }
			    catch (FormatException e)
			    {
				    logger.LogWarning(e, "Failed to parse {Input} to an cost", input);
			    }

			    return output;
		    });
	    }

	    async ValueTask<Category> GetCategory()
	    {
		    return await Task.Run(() =>
		    {
			    var catAsString = baseWeapon.CategoryAsString;
			    Category outCategory;
			    
			    if (Category.CategoryDictionary.ContainsKey(catAsString))
			    {
				    outCategory = Category.GetCategory(catAsString, logger);
				    //If WeaponType is not string.empty it should replace any category defined type.
				    if (!baseWeapon.WeaponType.IsNullOrEmpty())
					    outCategory.Type = baseWeapon.WeaponType;
				    //If WeaponType is still string.empty, try to set it from the auto detected category
				    if (baseWeapon.WeaponType.IsNullOrEmpty())
					    baseWeapon.WeaponType = outCategory.Type;

				    return outCategory;
			    }

			    outCategory = new Category()
			    {
				    Name = catAsString,
				    Type = baseWeapon.WeaponType
			    };

			    if (Category.CategoryDictionary.ContainsKey(catAsString)) 
				    return outCategory;

			    Category.CategoryDictionary.Add(outCategory.Name, outCategory);
			    return outCategory;

		    });
	    }

	    async ValueTask<Skill> GetSkill()
	    {
		    return await Task.Run(() =>
		    {
			    var skillDic = Skill.SkillDictionary;

			    if (skillDic is null)
				    throw new ArgumentNullException(nameof(skillDic), "SkillDictionary is not formed");

			    //If a useskill is defined it overrides everything
			    if (skillDic.ContainsKey(baseWeapon.UseSkillString))
				    return skillDic[baseWeapon.UseSkillString];

			    //Try to catch the cases in which skill == weapon category
			    if (skillDic.ContainsKey(baseWeapon.CategoryAsString))
				    return skillDic[baseWeapon.CategoryAsString];

			    //Try to detect skill by Category
			    switch (baseWeapon.CategoryAsString)
			    {
				    case "Unarmed" or "Bio-Weapon" or "Cyberweapon":
					    return skillDic["Unarmed Combat"];
				    case "Bows" or "Crossbows":
					    return skillDic["Archery"];
				    case "Tasers" or "Light Pistols" or "Heavy Pistols":
					    return skillDic["Pistols"];
				    case "Machine Pistols" or "Submachine Guns" or "Assault Rifles" or "Carbines"
					    or "Light Machine Guns" or "Medium Machine Guns" or "Heavy Machine Guns":
					    return skillDic["Automatics"];
				    case "Sporting Rifles" or "Sniper Rifles" or "Shotguns":
					    return skillDic["Longarms"];
				    case "Assault Cannons" or "Grenade Launchers" or "Missile Launchers":
					    return skillDic["Heavy Weapons"];
				    default:
					    logger.LogDebug("Could not parse useskill for {Category.Name} of {Name}",
						    baseWeapon.CategoryAsString, Name);
					    return new Skill() {Name = "Undefined Skill"};
			    }
		    });
	    }

	    async ValueTask<List<Accessory>> GetAccessories()
	    {
		    return await Task.Run(() => baseWeapon.AccessoriesNameList.Select(accessoryWithName => Accessory.GetAccessory(accessoryWithName, logger)).ToList());
	    }

    }
}
