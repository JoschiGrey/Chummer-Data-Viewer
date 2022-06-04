using System.Xml.Serialization;
using ChummerDataViewer.Extensions;
using ChummerDataViewer.Interfaces;

namespace ChummerDataViewer.Classes;

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
    
    public async Task CreateAsync(ILogger logger, ICreatable? baseObject = null)
    {
        await Task.Run(() => CategoryDictionary.Add(Name, this));
;
    }

    public static Category GetCategory(string name, ILogger logger)
    {
        return CategoryDictionary.GetValueByString(name, logger);
    }
}
