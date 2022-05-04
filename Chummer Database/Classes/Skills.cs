using System.Xml.Serialization;

namespace Chummer_Database.Classes;



public class Skill { 

    [XmlElement(ElementName="id")] 
    public Guid Id { get; set; }

    [XmlElement(ElementName = "name")] 
    public string Name { get; set; } = string.Empty;

    [XmlElement(ElementName = "attribute")]
    public string Attribute { get; set; }

    [XmlElement(ElementName="category")] 
    public string CategoryAsString { get; set; } = string.Empty;

    [XmlIgnore]
    public Category? Category { get; set; }

    [XmlElement(ElementName="default")] 
    private bool DefaultAble { get; set; }

    [XmlElement(ElementName = "skillgroup")]
    public string SkillGroup { get; set; } = string.Empty;

    [XmlArray("specs")]
    [XmlArrayItem("spec", typeof(string))]
    public HashSet<string> Specialisations { get; set; } = new();

    [XmlElement(ElementName = "source")] 
    public string Source { get; set; } = string.Empty;

    [XmlElement(ElementName="page")] 
    public int Page { get; set; } 

    [XmlElement(ElementName="exotic")] 
    private string ExoticString { get; set; } 
    
    [XmlIgnore]
    public bool Exotic => ExoticString.Equals("True");

    [XmlElement(ElementName="requiresflymovement")] 
    private bool RequiresFlyMovement { get; set; }

    [XmlElement(ElementName="requiresgroundmovement")] 
    private bool RequiresGroundMovement { get; set; } 
    

    [XmlElement(ElementName="requiresswimmovement")] 
    private bool RequiresSwimMovement { get; set; }
    

    public bool Create(ILogger logger)
    {
        if (XmlLoader.SkillsXmlData != null && XmlLoader.SkillsXmlData.SkillCategoriesDictionary.ContainsKey(CategoryAsString))
        {
            Category = XmlLoader.SkillsXmlData.SkillCategoriesDictionary[CategoryAsString];
        }

        return true;
    }
}