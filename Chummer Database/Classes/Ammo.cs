using System.Text.RegularExpressions;
using Blazorise.Extensions;
using Chummer_Database.Enums;
using Chummer_Database.Extensions;

namespace Chummer_Database.Classes;

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable UnusedAutoPropertyAccessor.Global

public class Ammo
{
    private string AmmoString { get; }
    public string DisplayString { get; }
    public bool NeedsEnergy { get; }
    public List<MagazineType> MagazineType { get; } = new();
    public int MagazineSize { get; }
    public string AmmoCategory { get; }
    private ILogger Logger { get; }

    public Ammo(Weapon weapon, ILogger logger)
    {
        Logger = logger;
        AmmoString = weapon.AmmoStringDeserialized;
        AmmoCategory = GetAmmoCategory();

        const string matchNumbers = @"\d+";
        var size = Regex.Match(AmmoString, matchNumbers).ToString();
        
        if(int.TryParse(size, out var result))
            MagazineSize = result;

        if (AmmoString.Contains("Energy"))
            NeedsEnergy = true;

        try
        {
            const string matchInsideBrackets = @"(?<=\().+?(?=\))";
            var matches = Regex.Matches(AmmoString, matchInsideBrackets);
            foreach (var match in matches)
            {
                var magString = match.ToString()?.ToLower();

                if(!string.IsNullOrWhiteSpace(magString))
                    MagazineType.Add(EnumReflection.GetEnumByDescription<MagazineType>(magString));
            }
        }
        catch (ArgumentException e)
        {
            Logger.LogWarning(e, "Could not parse AmmoType of {Name} with the string {AmmoString}", weapon.Name, AmmoString);
        }

        //Maybe I should just create a interface to enforce this? This is a placeholder right now.
        DisplayString = AmmoString;
        
        string GetAmmoCategory()
        {
            if (!weapon.AmmoCategoryDeserialized.IsNullOrEmpty() && weapon.AmmoCategoryDeserialized is not null)
                return weapon.AmmoCategoryDeserialized;
            
            if (weapon.AmmoStringDeserialized != "-" && weapon.RangeType != "Melee")
                return weapon.CategoryAsString;

            return string.Empty;
        }
    }
}


