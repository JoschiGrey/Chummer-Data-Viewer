using System.Xml.Serialization;
using Chummer_Database.Enums;

namespace Chummer_Database.Classes;

[XmlRoot(ElementName="accessory")]
public class Accessory { 

    [XmlElement(ElementName="rating")] 
    public int Rating { get; set; } 

    [XmlElement(ElementName="rc")] 
    public int Rc { get; set; } 

    [XmlElement(ElementName="rcdeployable")] 
    public bool Rcdeployable { get; set; } 

    [XmlElement(ElementName="rcgroup")] 
    public int Rcgroup { get; set; } 

    [XmlElement(ElementName="id")] 
    public string Id { get; set; } 

    [XmlElement(ElementName="name")] 
    public string Name { get; set; }

    [XmlElement(ElementName="avail")] 
    public int Avail { get; set; } 

    [XmlElement(ElementName="cost")] 
    public int Cost { get; set; } 

    [XmlElement(ElementName="source")] 
    public string Source { get; set; } 

    [XmlElement(ElementName="page")] 
    public int Page { get; set; } 
    
    [XmlElement(ElementName = "mount")]
    private string MountString { get; set; }
    
    [XmlIgnore] 
    public List<AccessoryMount> Mounts { get; set; } = new();
}
