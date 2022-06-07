using System.Text;
using System.Text.RegularExpressions;
using Blazorise.Extensions;
using ChummerDataViewer.Enums;
using ChummerDataViewer.Classes;
using ChummerDataViewer.Classes.HelperMethods;

namespace ChummerDataViewer.Extensions;

public static class WeaponExtensions
{
    public static string FormCostCssClass(this XmlWeapon xmlWeapon)
    {
        var sb = new StringBuilder();

        if (xmlWeapon.CostString.Contains("{Rating}*"))
            sb.Append("rating-times ");

        sb.Append("add-nuyen ");
        
        return sb.ToString();
    }
}