using System.Text.RegularExpressions;
using Blazorise.Extensions;
using ChummerDataViewer.Classes.HelperMethods;
using ChummerDataViewer.Enums;
using ChummerDataViewer.Extensions;
using ChummerDataViewer.Overrides;

namespace ChummerDataViewer.Classes;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

public class Ammo
{
    private string AmmoString { get; } = string.Empty;
    public string DisplayString => ToString();
    public bool NeedsEnergy { get; }
    public List<MagazineType> MagazineType { get; private set; } = new();
    public int MagazineSize { get; private set; }
    public string AmmoCategory { get; } = string.Empty;

    private bool Replace { get; }

    private bool Add { get; }

    public static HashSet<string> AllAmmoCategories { get; } = new();

    public Ammo(XmlAccessory accessory, ILogger logger)
    {
        if (!string.IsNullOrEmpty(accessory.AmmoReplace))
        {
            Replace = true;
            MagazineSize = RegexHelper.GetInt(accessory.AmmoReplace, logger);
            MagazineType = GetMagazineType(accessory.AmmoReplace, logger);
            return;
        }

        Add = true;
        MagazineSize = RegexHelper.GetInt(accessory.AmmoBonus, logger);

    }
    
    public Ammo(XmlWeapon weapon, ILogger logger)
    {
        AmmoString = weapon.AmmoStringDeserialized;
        AmmoCategory = GetAmmoCategory();

        if (!AllAmmoCategories.Contains(AmmoCategory) && AmmoCategory != string.Empty)
            AllAmmoCategories.Add(AmmoCategory);

        MagazineSize = RegexHelper.GetInt(weapon.AmmoStringDeserialized, logger);

        if (AmmoString.Contains("Energy"))
            NeedsEnergy = true;

        MagazineType = GetMagazineType(weapon.AmmoStringDeserialized, logger);

        string GetAmmoCategory()
        {
            if (!weapon.AmmoCategoryDeserialized.IsNullOrEmpty() && weapon.AmmoCategoryDeserialized is not null)
                return weapon.AmmoCategoryDeserialized;
            
            if (weapon.AmmoStringDeserialized != "-" && weapon.RangeType != "Melee")
                return weapon.CategoryAsString;

            return string.Empty;
        }
    }

    public override string ToString()
    {
        return string.Empty;
    }
    
    private static List<MagazineType> GetMagazineType(string ammoString, ILogger logger)
    {
        var outList = new List<MagazineType>();
        

        const string matchInsideBrackets = @"(?<=\().+?(?=\))";
        var matches = Regex.Matches(ammoString, matchInsideBrackets);
        foreach (var match in matches)
        {
            var magString = match.ToString()?.ToLower();

            try
            {
                if (!string.IsNullOrWhiteSpace(magString))
                    outList.Add(EnumReflection.GetEnumByDescription<MagazineType>(magString));
            }
            catch (ArgumentException e)
            {
                logger.LogWarning(e, "Could not parse AmmoType with of {AmmoString}", ammoString);
            }
            

        }

        return outList;

    }

    public void ApplyAccessoryModification(Ammo accessoryAmmoObject)
    {
        if (accessoryAmmoObject.Replace)
        {
            MagazineSize = accessoryAmmoObject.MagazineSize;
            MagazineType = accessoryAmmoObject.MagazineType;
            return;
        }

        MagazineSize += accessoryAmmoObject.MagazineSize;
    }

    public void ModifyWithExpression(string expression)
    {
        var expressionString =  MagazineSize + " " + expression;
        var evaluator = new CustomExpressionEvaluator
        {
            Variables = new Dictionary<string, object>()
            {
                {"Weapon", MagazineSize}
            }
        };
        var newAmmoSize = evaluator.Evaluate<int>(expressionString);

        MagazineSize = newAmmoSize;
    }
}


