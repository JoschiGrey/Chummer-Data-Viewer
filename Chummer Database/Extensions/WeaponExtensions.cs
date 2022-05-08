using System.Text;
using System.Text.RegularExpressions;
using Blazorise.Extensions;
using Chummer_Database.Classes;
using Chummer_Database.Enums;

namespace Chummer_Database.Extensions;

public static class WeaponExtensions
{
    private const string PlusRegexPattern = @"\+(?=[0-9])";
    
    public static string FormAvailabilityCssClass(this Weapon weapon)
    {
        StringBuilder cssStringBuilder = new StringBuilder();

        if (weapon.Availability is null)
            return string.Empty;

        var avail = weapon.Availability;
        switch (avail.Legality)
        {
            case Legality.Forbidden:
                cssStringBuilder.Append("forbidden ");
                break;
            case Legality.Restricted:
                cssStringBuilder.Append("restricted ");
                break;
        }

        if (avail.AvailabilityString.Contains("{Rating}*"))
            cssStringBuilder.Append("rating-times ");

        if (avail.AvailabilityString.Contains("{Rating}+"))
            cssStringBuilder.Append("rating-plus ");

        if (Regex.IsMatch(avail.AvailabilityString, PlusRegexPattern))
            cssStringBuilder.Append("prefix-plus ");


        return cssStringBuilder.ToString();
    }

    public static string FormCostCssClass(this Weapon weapon)
    {
        var sb = new StringBuilder();

        if (weapon.CostString.Contains("{Rating}*"))
            sb.Append("rating-times ");

        sb.Append("add-nuyen ");
        
        return sb.ToString();
    }
    
    public static string FormDamageCssString(this Weapon weapon)
    {
        if (weapon.Damage is null)
            return string.Empty;
        
        if (weapon.Damage.FullDamageString.IsNullOrEmpty())
            return string.Empty;
        
        //Osmium Mace, yeah not gonna deal with that shit rn!
        if (weapon.Id == new Guid("78beeb28-9ae6-418d-97bb-1d45ac19cee8"))
            return "special ";

        var fullDamageString = weapon.Damage.FullDamageString;
        
        var sb = new StringBuilder();
        switch (fullDamageString)
        {
            case { } s when s.Contains("MAG*"):
                sb.Append("dmg-mag-times ");
                break;
            case { } s when s.Contains("Rating+"):
                sb.Append("rating-plus ");
                break;
            case { } s when s.Contains("{STR}+"):
                sb.Append("str-plus ");
                break;
            case { } s when s.Contains("{STR}"):
                sb.Append("str ");
                break;
            case { } s when s.Contains("Special"):
                sb.Append("special ");
                break;
            case { } s when s.Contains("Grenade"):
                sb.Append("grenade ");
                break;
            case { } s when s.Contains("Missile"):
                sb.Append("missile ");
                break;
            case { } s when s.Contains("Chemical"):
                sb.Append("chemical ");
                break;
            case { } s when s.Contains("Toxin"):
                sb.Append("chemical ");
                break;
            case { } s when s.Contains("Pepper Punch"):
                sb.Append("chemical ");
                break;
        }

        switch (weapon.Damage.DamageType)
        {
            case DamageType.Physical:
                sb.Append("dmg-physical ");
                break;
            case DamageType.Stun:
                sb.Append("dmg-stun ");
                break;
        }

        switch (weapon.Damage.Element)
        {
            case Element.Electro:
                sb.Append("dmg-electro ");
                break;
        }

        return sb.ToString();
    }
}