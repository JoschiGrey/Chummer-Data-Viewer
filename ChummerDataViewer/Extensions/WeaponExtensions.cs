using System.Text;
using System.Text.RegularExpressions;
using Blazorise.Extensions;
using ChummerDataViewer.Enums;
using ChummerDataViewer.Classes;

namespace ChummerDataViewer.Extensions;

public static class WeaponExtensions
{
    private const string PlusRegexPattern = @"\+(?=[0-9])";

    public static string FormCostCssClass(this XmlWeapon xmlWeapon)
    {
        var sb = new StringBuilder();

        if (xmlWeapon.CostString.Contains("{Rating}*"))
            sb.Append("rating-times ");

        sb.Append("add-nuyen ");
        
        return sb.ToString();
    }
}