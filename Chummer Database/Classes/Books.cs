using System.Xml.Serialization;

namespace Chummer_Database.Classes;


[XmlRoot("book")]
public class Book {
    [XmlElement(ElementName="id")] 
    public Guid Id { get; set; }

    [XmlElement(ElementName = "name")] 
    public string Name { get; set; } = string.Empty;

    [XmlElement(ElementName = "code")] 
    public string Code { get; set; } = string.Empty;
}
