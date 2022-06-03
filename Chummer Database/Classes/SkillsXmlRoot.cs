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
    public Dictionary<string, Skill> SkillsDictionary { get; set; } = new();

    [XmlIgnore]
    public Dictionary<string, Category> SkillCategoriesDictionary = new();

    public bool Create(ILogger logger)
    {
        logger.LogInformation("Creating {Type}", GetType().Name);


        SkillCategoriesDictionary = SkillCategories.ToDictionary(k => k.Name);

        SkillsDictionary = Skills.ToDictionary(k => k.Name);

        foreach (var skill in Skills)
        {
            skill.Create(logger);
        }

        return true;
    }
    
    public async Task<ICreatable> CreateAsync(ILogger logger, ICreatable? baseObject)
    {
        await Task.Run(() =>
        {
            Create(logger);
            XmlLoader.CreatedXml.Add(GetType());
        });
        logger.LogInformation("Created {Type}", GetType().Name);
        return this;
    }

    private static HashSet<Type> Dependencies { get; set; } = new HashSet<Type>() {typeof(BooksXmlRoot)};
    public bool CheckDependencies()
    {
        return Dependencies.IsSubsetOf(XmlLoader.CreatedXml);
    }
}