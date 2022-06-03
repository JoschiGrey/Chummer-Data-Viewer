using System.Xml.Serialization;
using Chummer_Database.Interfaces;

namespace Chummer_Database.Classes;

[XmlRoot("chummer")]
public class RangesXmlRoot: ICreatable
{
    [XmlArray("ranges")]
    [XmlArrayItem("range")]
    public List<WeaponRange> Ranges { get; set; } = new();

    [XmlIgnore]
    public Dictionary<string, WeaponRange> RangeDictionary { get; set; } = new();

    public bool Create(ILogger logger)
    {
        logger.LogInformation("Creating {Type}", GetType().Name);
        
        RangeDictionary = Ranges.ToDictionary(k => k.RangeCategory);

        return true;
    }

    public async Task<ICreatable> CreateAsync(ILogger logger, ICreatable? baseObject)
    {
        await Task.Run(() =>
        {
            Create(logger);
            XmlLoader.CreatedXml.Add(GetType());
        });
        logger.LogInformation("Created {Type}", GetType().Name);
        return this;
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

    public static WeaponRange GetWeaponRange(string rangeCategory)
    {
        if (XmlLoader.RangesXmlData is null)
            throw new ArgumentException(nameof(XmlLoader.RangesXmlData));
        if (XmlLoader.RangesXmlData.RangeDictionary is null)
            throw new ArgumentException(nameof(XmlLoader.RangesXmlData.RangeDictionary));

        try
        {
            return XmlLoader.RangesXmlData.RangeDictionary[rangeCategory];
        }
        catch (KeyNotFoundException e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}