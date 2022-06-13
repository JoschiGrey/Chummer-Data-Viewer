using System.Collections.Immutable;
using Blazorise.Extensions;
using ChummerDataViewer.Backend.Classes.HelperMethods;
using ChummerDataViewer.Backend.Enums;
using ChummerDataViewer.Backend.Extensions;
using ChummerDataViewer.Backend.Interfaces;

// ReSharper disable CommentTypo


namespace ChummerDataViewer.Backend.Classes;

public sealed record Weapon : ICreatable, IHoldAccessories
{
	public Guid Id { get; init; }
    
    public string Name { get; private set; } = string.Empty;
    
    public Category? Category { get; private set; }
    
    public int Accuracy { get; private set; }
    
    public Damage? Damage { get; private set; }
    
    public ArmorPen? ArmorPen { get; private set; }
    
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
    
    //TODO:This should be reworked into it's own class maybe an enum?
    public string FiringMode { get; private set; } = string.Empty;
    
    /// <summary>
    /// Stores a weapon object with all original values, that could have been changed by an accessory.
    /// </summary>
    public Weapon? BackupWeapon { get; set; }

    /// <summary>
    /// All Mounts
    /// </summary>
    public HashSet<MountSlot> MountSlots { get; set; } = new HashSet<MountSlot>();
    
    //Melee Exclusive
    public int Reach { get; private set; }

    public IReadOnlySet<Accessory> BaseAccessories { get; private set; }

    public string CostCss { get; private set; } = string.Empty;
    
    public static HashSet<Weapon> AllWeapons { get; } = new();
    
    
    /// <summary>
    /// This copy Constructor saves all the values an accessory could change into a backup weapon object
    /// </summary>
    /// <param name="templateWeapon"></param>
    private Weapon(Weapon templateWeapon)
    {
	    Availability = templateWeapon.Availability;
	    Cost = templateWeapon.Cost;
	    RecoilCompensation = templateWeapon.RecoilCompensation;
	    Conceal = templateWeapon.Conceal;
	    Accuracy = templateWeapon.Accuracy;
	    Ammo = templateWeapon.Ammo;
	    _accessoryCostMultiplier = templateWeapon._accessoryCostMultiplier;
	    MountSlots = templateWeapon.MountSlots;
	    //TODO: True Unberbarrelweapon Support
	    Damage = templateWeapon.Damage;
	    ArmorPen = templateWeapon.ArmorPen;
	    Reach = templateWeapon.Reach;
    }

    public Weapon()
    {
	    
    }

    public async Task CreateAsync(ILogger logger, ICreatable? baseObject)
    {
	    //TODO: Add underbarrel support. Make AllWeapons into an observablecollection and wait until the correct weapon was filled?
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

		    foreach (var mount in baseWeapon.AccessoryMounts)
		    {
			    MountSlots.Add(new MountSlot(mount));
		    }

		    Category = await GetCategory();
		    Range = WeaponRange.GetWeaponRange(baseWeapon.WeaponRangeString, Category.Name);
		    AlternateWeaponRange = WeaponRange.GetWeaponRange(baseWeapon.AlternateRangeString, Category.Name);

		    ArmorPen = new ArmorPen(baseWeapon.ApString);
		    
		    RecoilCompensation = await Task.Run(() => RegexHelper.GetInt(baseWeapon.RecoilCompensationString, logger)).ConfigureAwait(false);
		    
		    Skill = await GetSkill().ConfigureAwait(false);
		    
		    Cost = await Task.Run(() => RegexHelper.GetInt(baseWeapon.CostString, logger)).ConfigureAwait(false);
		    
		    Accuracy = await Task.Run(() => RegexHelper.GetInt(baseWeapon.AccuracyString, logger)).ConfigureAwait(false);
		    
		    CostCss = baseWeapon.FormCostCssClass();

		    await AddAccessoriesAsync();
		    
		    AllWeapons.Add(this);
	    }
	    catch (Exception e)
	    {
		    logger.LogCritical(e, "Creation failed of {Type}, {Name}", GetType(), Name);
		    throw;
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

	    async Task AddAccessoriesAsync()
	    {
		    BaseAccessories = baseWeapon.AccessoriesNameList
			    .Select(accessoryWithName => Accessory.GetAccessory(accessoryWithName.Name, logger)).ToImmutableHashSet();

		    if(await TryAddMultipleAccessoriesAsync(BaseAccessories, logger, true))
			    logger.LogDebug("Could not gracefully add all accessories for {Name}", Name);
	    }

    }

    
    public async ValueTask<bool> TryAddAccessoryAsync(Accessory accessory, ILogger logger, bool forceAdd = false)
    {
	    if (!accessory.PossibleMountSlots.Any())
	    {
		    await ForceAddAccessoryAsync(accessory, logger);
		    return true;
	    }


		if (!accessory.TryGetFreeSlots(this, logger, out var fittingSlots))
	    {
		    if (forceAdd)
			    await ForceAddAccessoryAsync(accessory, logger);
		    
		    return false;
	    }
		   
	    var firstFreeSlot = fittingSlots.First();



	    await AddAccessory(accessory, firstFreeSlot, logger);
	    return true;
    }

    /// <summary>
    /// Tries to add a range of accessories. Returns false if an accessory could not be added gracefully
    /// </summary>
    /// <param name="accessories"></param>
    /// <param name="logger"></param>
    /// <param name="forceAdd">If no slot is free, a slot will be generated for that accessory</param>
    /// <returns></returns>
    public async ValueTask<bool> TryAddMultipleAccessoriesAsync(IEnumerable<Accessory> accessories, ILogger logger, bool forceAdd = false)
    {
	    var accessoryList = accessories.ToList();
	    accessoryList.Sort((x, y) => x.PossibleMountSlots.Count.CompareTo(y.PossibleMountSlots.Count));
	    
	    //TODO: Das Folgende Problem Lösen
	    // Waffe: Barrel, Top, Side
	    //1: Top, Side
	    //2: Barrel, Top
	    //3: Barrel, Top -> Passt nicht mehr obwohl es in einer anderen Reihenfolge gepasst hätte
	    //Lösung 1: Sortieren nach noch freien plätzen für diesen Aufsatz : Mölgliche Plätze des Accessory - Verbrauchte Plätze der Waffe des selben Slots
	    //Läsung 2: Befüllen nach Slots nicht nach Accessories, aber selbes sortier Prinzip

	    var addedAll = true;
	    foreach (var accessory in accessoryList)
	    {
		    if (!await TryAddAccessoryAsync(accessory, logger, forceAdd))
			    addedAll = false;
	    }
	    return addedAll;
    }

    public async ValueTask AddAccessory(Accessory accessory, MountSlot mountSlot, ILogger logger)
    {
	    mountSlot.Accessory = accessory;
	    await ApplyAccessoryAsync(accessory, logger);
    }

    public async ValueTask ForceAddAccessoryAsync(Accessory accessory, ILogger logger)
    {
	    MountSlot forcedSlot;
	    //Handles internal mods.
	    if (!accessory.PossibleMountSlots.Any())
		    forcedSlot = new MountSlot(WeaponMountSlots.Internal){IsForced = true};

	    else
		    forcedSlot = new MountSlot(accessory.PossibleMountSlots.First()){IsForced = true};
	    
	    MountSlots.Add(forcedSlot);
	    await AddAccessory(accessory, forcedSlot, logger);
    }

    public void RemoveAccessory(Accessory accessory, MountSlot mountSlot, ILogger logger)
    {
	    throw new NotImplementedException();
    }

    /// <summary>
    /// Key:RcGroup Value:Modification
    /// </summary>
    private readonly Dictionary<int, int> _usedRcGroups = new();

    private int _accessoryCostMultiplier = 1;
    private async Task ApplyAccessoryAsync(Accessory accessory, ILogger logger)
    {
	    BackupWeapon ??= new Weapon(this);

	    //If an acessory is part of the base weapon it does not modify availability.
	    if(accessory.Availability is not null && Availability is not null && !BaseAccessories.Contains(accessory))
			Availability += accessory.Availability;

	    //If an acessory is part of the base weapon it does not modify cost
	    if(!BaseAccessories.Contains(accessory))
			Cost += accessory.Cost;

	    await Task.Run(ModifyRecoil);

	    Conceal += accessory.ConcealModification;

	    Accuracy += accessory.Accuracy;
		
	    if(Ammo is not null && accessory.AmmoModification is not null)
			Ammo.ApplyAccessoryModification(accessory.AmmoModification);

	    if (!string.IsNullOrEmpty(accessory.ModifyAmmoCapacity) && Ammo is not null)
	    {
			Ammo.ModifyWithExpression(accessory.ModifyAmmoCapacity);
	    }

	    _accessoryCostMultiplier *= accessory.AccessoryCostMultiplier;

	    foreach (var mount in accessory.ExtraMount)
	    {
		    MountSlots.Add(new MountSlot(mount));
	    }
	    
	    //TODO: True Unberbarrelweapon Support

	    if (Damage is not null) 
		    Damage.DamageAmount += accessory.DamageAmountModification;

	    if (Damage is not null && accessory.DamageTypeOverride.Equals("P"))
		    Damage.DamageType = DamageType.Physical;

	    if (ArmorPen is not null) 
		    ArmorPen.ApModifier = accessory.ArmorPenModification;

	    Reach += accessory.ReachModifier;

	    void ModifyRecoil()
	    {
		    if (accessory.RecoilModification == 0) return;

		    //No Group => Always applicable
		    if (accessory.RcGroup == 0)
		    {
			    RecoilCompensation += accessory.RecoilModification;
			    return;
		    }

		    //No Mod of that group present => Add it to the dic and add Mod to weapon
		    if (!_usedRcGroups.ContainsKey(accessory.RcGroup))
		    {
			    _usedRcGroups.Add(accessory.RcGroup, accessory.RecoilModification);
			    RecoilCompensation += accessory.RecoilModification;
			    return;
		    }
		    
		    //Always keep the higher modification
		    if(accessory.RecoilModification <= _usedRcGroups[accessory.RcGroup])
			    return;

		    _usedRcGroups[accessory.RcGroup] = accessory.RecoilModification;
		    RecoilCompensation += accessory.RecoilModification;
	    }
    }


}
