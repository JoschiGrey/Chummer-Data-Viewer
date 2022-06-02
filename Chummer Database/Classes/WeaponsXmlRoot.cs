using System.Diagnostics.CodeAnalysis;
using System.Xml.Serialization;
using Blazor.Extensions.Logging;
using Chummer_Database.Interfaces;
using Microsoft.AspNetCore.Components;
using NullGuard;

namespace Chummer_Database.Classes;

[XmlRoot("chummer")]
public class WeaponsXmlRoot : ICreatable, IHasDependency
{
    [XmlArray("categories")]
    [XmlArrayItem("category", typeof(Category))]
    public List<Category> WeaponCategories { get; set; } = new();
    public static Dictionary<string, Category> WeaponCategoryDictionary { get; private set; } = new();
    
    [XmlArray("weapons")]
    [XmlArrayItem("weapon")]
    public List<Weapon> Weapons { get; set; } = new();

    [XmlArray("accessories")]
    [XmlArrayItem("accessory", typeof(Accessory))]
    public List<Accessory> Accessories { get; set; } = new();

    [XmlIgnore]
    public static Dictionary<string, Accessory> AccessoriesDictionary { get; private set; } = new();
    public bool Create(ILogger logger)
    {
        logger.LogInformation("Creating {Type}", GetType().Name);
        
        WeaponCategoryDictionary = WeaponCategories.ToDictionary(k => k.Name);
        AccessoriesDictionary = Accessories.ToDictionary(k => k.Name);
        
        
        foreach (var accessory in Accessories)
        {
            accessory.Create(logger);
        }
        
        foreach (var weapon in Weapons)
        {
            weapon.Create(logger);
        }



        return true;
    }
    
    public async Task<ICreatable> CreateAsync(ILogger logger)
    {
        await Task.Run(() =>
        {
            Create(logger);
            XmlLoader.CreatedXml.Add(GetType());
        });
        logger.LogInformation("Created {Type}", GetType().Name);
        return this;
    }

    
    private static HashSet<Type> Dependencies { get; set; } = new HashSet<Type>()
        {typeof(BooksXmlRoot), typeof(RangesXmlRoot), typeof(SkillsXmlRoot)};
    public bool CheckDependencies()
    {
        return Dependencies.IsSubsetOf(XmlLoader.CreatedXml);
    }
}