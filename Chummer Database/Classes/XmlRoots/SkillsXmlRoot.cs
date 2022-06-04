using System.Xml.Serialization;
using Chummer_Database.Interfaces;

namespace Chummer_Database.Classes;

[XmlRoot("chummer")]
public class SkillsXmlRoot : ICreatable, IHasDependency
{
    [XmlArray("skillgroups")]
    [XmlArrayItem("name", typeof(string))]
    public HashSet<string> SkillGroups { get; set; } = new();

    [XmlArray("categories")]
    [XmlArrayItem("category", typeof(Category))]
    public HashSet<Category> SkillCategories { get; set; } = new();

    [XmlArray("skills")]
    [XmlArrayItem("skill", typeof(Skill))]
    public HashSet<Skill> Skills { get; set; } = new();
    

    [XmlIgnore]
    public Dictionary<string, Category> SkillCategoriesDictionary = new();

    public async Task CreateAsync(ILogger logger, ICreatable? baseObject)
    {
        SkillCategoriesDictionary = SkillCategories.ToDictionary(k => k.Name);

        var taskList = new List<Task>();
        foreach (var category in SkillCategories)
        {
           taskList.Add(category.CreateAsync(logger));
        }
        await Task.WhenAll(taskList);

        foreach (var skill in Skills)
        {
            taskList.Add(skill.CreateAsync(logger));
        }
        await Task.WhenAll(taskList);
        
            
        XmlLoader.CreatedXml.Add(GetType());

        logger.LogInformation("Created {Type}", GetType().Name);

    }

    private static HashSet<Type> Dependencies { get; set; } = new HashSet<Type>() {typeof(BooksXmlRoot)};
    public bool CheckDependencies()
    {
        return Dependencies.IsSubsetOf(XmlLoader.CreatedXml);
    }
}