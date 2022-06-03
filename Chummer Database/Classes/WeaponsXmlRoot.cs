using System.Xml.Serialization;
using Chummer_Database.Interfaces;
namespace Chummer_Database.Classes;

[XmlRoot("chummer")]
public class WeaponsXmlRoot : ICreatable, IHasDependency
{
    [XmlArray("categories")]
    [XmlArrayItem("category", typeof(Category))]
    public List<Category> WeaponCategories { get; set; } = new();
    
    [XmlArray("weapons")]
    [XmlArrayItem("weapon", typeof(XmlWeapon))]
    public List<XmlWeapon> Weapons { get; set; } = new();


    [XmlArray("accessories")]
    [XmlArrayItem("accessory", typeof(Accessory))]
    public List<Accessory> Accessories { get; set; } = new();
    
    [XmlIgnore]
    private static HashSet<Type> Dependencies { get; set; } = new()
        {typeof(BooksXmlRoot), typeof(RangesXmlRoot), typeof(SkillsXmlRoot)};
    
    public async Task Create(ILogger logger)
    {
        logger.LogInformation("Creating {Type}", GetType().Name);

        var taskList = new List<Task>();

        foreach (var category in WeaponCategories)
        {
            taskList.Add(category.CreateAsync(logger));
        }
        
        foreach (var accessory in Accessories)
        {
            taskList.Add(accessory.CreateAsync(logger));
        }
        
        foreach (var weapon in Weapons)
        {
            taskList.Add(weapon.CreateAsync(logger, null));
        }

        await Task.WhenAll(taskList);
    }
    
    public async Task<ICreatable> CreateAsync(ILogger logger, ICreatable? baseObject)
    {

        await Create(logger);
        XmlLoader.CreatedXml.Add(GetType());

        logger.LogInformation("Created {Type}", GetType().Name);
        return this;
    }
    
    public bool CheckDependencies()
    {
        return Dependencies.IsSubsetOf(XmlLoader.CreatedXml);
    }
}