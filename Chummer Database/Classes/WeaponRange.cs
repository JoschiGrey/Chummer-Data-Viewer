using System.Xml.Serialization;
using Chummer_Database.Extensions;
using Chummer_Database.Interfaces;

namespace Chummer_Database.Classes;
public class WeaponRange : ICreatable
{
    [XmlElement("name")]
    public string Name { get; set; } = string.Empty;

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
    
    public static Dictionary<string, WeaponRange> RangeDictionary { get; set; } = new();

    public static WeaponRange GetWeaponRange(string rangeCategory, string weaponCategory)
    {
        if(RangeDictionary.ContainsKey(rangeCategory))
            return RangeDictionary[rangeCategory];

        foreach (var (key,value) in RangeDictionary)
        {
            if (key.Contains(rangeCategory))
                return value;
            if (key.Contains(weaponCategory))
                return value;
        }

        return new WeaponRange() {Name = "Undefined Range"};
    }

    public async Task CreateAsync(ILogger logger, ICreatable? baseObject = null)
    {
        await Task.Run(() => RangeDictionary.Add(Name, this));
    }
}