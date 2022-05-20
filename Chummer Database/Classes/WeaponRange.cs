﻿using System.Xml.Serialization;

namespace Chummer_Database.Classes;

[XmlRoot("chummer")]
public class RangesXmlRoot
{
    [XmlArray("ranges")]
    [XmlArrayItem("range")]
    public List<WeaponRange> Ranges { get; set; } = new();

    [XmlIgnore]
    public Dictionary<string, WeaponRange> RangeDictionary { get; set; } = new();

    public bool Create(ILogger logger)
    {
        logger.LogDebug("Creating {Type}", GetType().Name);
        
        RangeDictionary = Ranges.ToDictionary(k => k.RangeCategory);

        return true;
    }
}


public class WeaponRange
{
    [XmlElement("name")]
    public string RangeCategory { get; set; } = string.Empty;

    [XmlElement(ElementName = "min")] 
    public string Min { get; set; } = "0";

    [XmlElement(ElementName = "short")] 
    public string Short { get; set; } = "0";

    [XmlElement(ElementName="medium")] 
    public string Medium { get; set; } = "0";

    [XmlElement(ElementName="long")] 
    public string Long { get; set; } = "0";

    [XmlElement(ElementName="extreme")] 
    public string Extreme { get; set; } = "0";
}