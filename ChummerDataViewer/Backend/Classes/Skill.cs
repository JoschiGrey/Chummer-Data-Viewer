using System.Xml.Serialization;
using ChummerDataViewer.Backend.Extensions;
using ChummerDataViewer.Backend.Interfaces;

namespace ChummerDataViewer.Backend.Classes;



public class Skill : ICreatable { 

    [XmlElement(ElementName="id")] 
    public Guid Id { get; set; }

    [XmlElement(ElementName = "name")] 
    public string Name { get; set; } = string.Empty;

    [XmlElement(ElementName = "attribute")]
    public string Attribute { get; set; } = string.Empty;

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
    
    [XmlIgnore] 
    public string DisplaySource => $"p.{Page} ({Source})";

    [XmlElement(ElementName="exotic")] 
    private string ExoticString { get; set; } = string.Empty;
    
    [XmlIgnore]
    public bool IsExotic => ExoticString.Equals("True");

    [XmlElement(ElementName="requiresflymovement")] 
    private bool RequiresFlyMovement { get; set; }

    [XmlElement(ElementName="requiresgroundmovement")] 
    private bool RequiresGroundMovement { get; set; } 
    

    [XmlElement(ElementName="requiresswimmovement")] 
    private bool RequiresSwimMovement { get; set; }

    [XmlIgnore] 
    public static Dictionary<string, Skill> SkillDictionary { get; } = new();
    

    public Task CreateAsync(ILogger logger, ICreatable? baseObject = null)
    {
        Category.CategoryDictionary.GetValueByString(CategoryAsString, logger);
        
        SkillDictionary.Add(Name, this);

        return Task.CompletedTask;
    }
}