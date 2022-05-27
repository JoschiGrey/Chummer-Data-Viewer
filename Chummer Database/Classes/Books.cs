using System.Xml.Serialization;
using Chummer_Database.Interfaces;

namespace Chummer_Database.Classes;


[XmlRoot("book")]
public class Book : IDisplayable {
    [XmlElement(ElementName="id")] 
    public Guid Id { get; set; }

    [XmlElement(ElementName = "name")] 
    public string Name { get; set; } = string.Empty;

    [XmlElement(ElementName = "code")] 
    public string Code { get; set; } = string.Empty;

    public string DisplayName => Name;
}
