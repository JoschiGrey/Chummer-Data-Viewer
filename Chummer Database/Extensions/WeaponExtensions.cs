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
}