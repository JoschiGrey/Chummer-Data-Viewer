using System.Xml.Serialization;
using ChummerDataViewer.Interfaces;

namespace ChummerDataViewer.Classes.XmlRoots;

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
    [XmlArrayItem("accessory", typeof(XmlAccessory))]
    public List<XmlAccessory> Accessories { get; set; } = new();

    public IReadOnlySet<Type> Dependencies { get; } = new HashSet<Type>()
        {typeof(BooksXmlRoot), typeof(RangesXmlRoot), typeof(SkillsXmlRoot)};

    public async Task CreateAsync(ILogger logger, ICreatable? baseObject)
    {

        var taskList = new List<Task>();

        foreach (var category in WeaponCategories)
        {
            taskList.Add(category.CreateAsync(logger));
        }

        foreach (var accessory in Accessories)
        {
            taskList.Add(accessory.CreateAsync(logger));
        }
        
        await Task.WhenAll(taskList);
        
        foreach (var weapon in Weapons)
        {
            taskList.Add(weapon.CreateAsync(logger, null));
        }

        await Task.WhenAll(taskList).ConfigureAwait(false);
        
        XmlLoader.CreatedXml.Add(GetType());

        logger.LogInformation("Created {Type}", GetType().Name);

    }
    
    public bool CheckDependencies()
    {
        return Dependencies.IsSubsetOf(XmlLoader.CreatedXml);
    }
}