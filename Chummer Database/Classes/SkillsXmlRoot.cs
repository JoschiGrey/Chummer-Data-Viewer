using System.Xml.Serialization;

namespace Chummer_Database.Classes;

[XmlRoot("chummer")]
public class SkillsXmlRoot
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
        logger.LogDebug("Creating {Type}", GetType().Name);
        
        SkillCategoriesDictionary = SkillCategories.ToDictionary(k => k.Name);

        SkillsDictionary = Skills.ToDictionary(k => k.Name);

        foreach (var skill in Skills)
        {
            skill.Create(logger);
        }
        
        return true;
    }
}