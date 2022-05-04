using System.Xml.Serialization;

namespace Chummer_Database.Classes;

[XmlRoot("chummer")]
public class RangesXmlRoot
{
    [XmlArray("ranges")]
    [XmlArrayItem("range")]
    public List<Range> Ranges { get; set; } = new();

    [XmlIgnore]
    public Dictionary<string, Range> RangeDictionary { get; set; } = new();

    public bool Create(ILogger logger)
    {
        logger.LogDebug("Creating {Type}", GetType().Name);
        
        RangeDictionary = Ranges.ToDictionary(k => k.RangeCategory);

        return true;
    }
}


public class Range
{
    [XmlElement("name")]
    public string RangeCategory { get; set; } = string.Empty;
    
    [XmlElement(ElementName="min")] 
    public string Min { get; set; } 

    [XmlElement(ElementName="short")] 
    public string Short { get; set; } 

    [XmlElement(ElementName="medium")] 
    public string Medium { get; set; } 

    [XmlElement(ElementName="long")] 
    public string Long { get; set; } 

    [XmlElement(ElementName="extreme")] 
    public string Extreme { get; set; }
}