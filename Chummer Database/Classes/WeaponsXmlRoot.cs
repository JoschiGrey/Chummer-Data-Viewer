using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using Blazor.Extensions.Logging;
using Microsoft.AspNetCore.Components;
using NullGuard;

namespace Chummer_Database.Classes;

[XmlRoot("chummer")]
public class WeaponsXmlRoot
{
    [XmlArray("categories")]
    [XmlArrayItem("category", typeof(Category))]
    public List<Category> WeaponCategories { get; set; } = new();
    public static Dictionary<string, Category> WeaponCategoryDictionary { get; set; } = new();
    
    [XmlArray("weapons")]
    [XmlArrayItem("weapon")]
    public List<Weapon> Weapons { get; set; } = new();
    
    public bool Create(ILogger logger)
    {
        logger.LogTrace("Creating {Type}", GetType().Name);
        
        WeaponCategoryDictionary = WeaponCategories.ToDictionary(k => k.Name);
        
        
        foreach (var weapon in Weapons)
        {
            weapon.Create(logger);
        }

        return true;
    }
}