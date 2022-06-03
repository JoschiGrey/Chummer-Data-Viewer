using System.Xml.Serialization;
using Chummer_Database.Extensions;
using Chummer_Database.Interfaces;

namespace Chummer_Database.Classes;

[XmlRoot(ElementName = "category")]
[XmlType("category")]
public class Category : IDisplayable, ICreatable
{
    [XmlAttribute(AttributeName = "blackmarket")]
    public string BlackMarket { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "type")] public string Type { get; set; } = string.Empty;

    [XmlText] 
    public string Name { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "gunneryspec")]
    public string GunnerySpec { get; set; } = string.Empty;

    public string DisplayName => Name;
    
    public static Dictionary<string, Category> CategoryDictionary { get; private set; } = new();
    
    public async Task<ICreatable> CreateAsync(ILogger logger, ICreatable? baseObject = null)
    {
        await Task.Run(() => CategoryDictionary.Add(Name, this));
        return this;
    }

    public static Category GetCategory(string name)
    {
        return CategoryDictionary.GetValueByString(name);
    }
}
