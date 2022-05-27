using System.Xml.Serialization;
using Chummer_Database.Interfaces;

namespace Chummer_Database.Classes;

[XmlRoot(ElementName = "category")]
[XmlType("category")]
public class Category : IDisplayable
{
    [XmlAttribute(AttributeName = "blackmarket")]
    public string BlackMarket { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "type")] public string Type { get; set; } = string.Empty;

    [XmlText] 
    public string Name { get; set; } = string.Empty;

    [XmlAttribute(AttributeName = "gunneryspec")]
    public string GunnerySpec { get; set; } = string.Empty;

    public string DisplayName => Name;
}
