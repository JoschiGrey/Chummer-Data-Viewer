using System.Text;
using ChummerDataViewer.Backend.Classes;

namespace ChummerDataViewer.Backend.Extensions;

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