using System.Xml.Serialization;

namespace Chummer_Database.Classes;

[XmlRoot(ElementName="accessory")]
public class Accessory { 

    [XmlElement(ElementName="rating")] 
    public List<int> Rating { get; set; } 

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
}

[XmlRoot(ElementName="accessories")]
public class Accessories { 

    [XmlElement(ElementName="accessory")] 
    public List<Accessory> Accessory { get; set; } 
}