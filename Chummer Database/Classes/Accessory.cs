using System.Text.RegularExpressions;
using System.Xml.Serialization;
using Chummer_Database.Enums;
using Chummer_Database.Extensions;
using Chummer_Database.Interfaces;

namespace Chummer_Database.Classes;

[XmlRoot(ElementName="accessory")]
public class Accessory : ICreatable
{
    [XmlElement(ElementName="rating")] 
    public int Rating { get; set; } 
    
    [XmlElement(ElementName="rc")] 
    public int Rc { get; set; } 
    
    [XmlElement(ElementName="rcdeployable")] 
    public bool Rcdeployable { get; set; } 
    
    [XmlElement(ElementName="rcgroup")] 
    public int Rcgroup { get; set; } 
    
    [XmlElement(ElementName="id")] 
    public Guid Id { get; set; }
    
    [XmlElement(ElementName = "name")] 
    public string Name { get; set; } = string.Empty;
    
    [XmlElement(ElementName = "avail")] 
    public string Avail { get; set; } = string.Empty;
    
    [XmlElement(ElementName = "cost")] 
    public string Cost { get; set; } = string.Empty;
    
    [XmlElement(ElementName="source")] 
    public string Book { get; set; } = string.Empty;
    
    [XmlElement(ElementName="page")] 
    public int Page { get; set; }
    
    [XmlElement(ElementName = "mount")]
    private string MountString { get; set; } = string.Empty;
    
    
    [XmlIgnore] 
    public string DisplaySource => $"p.{Page} ({Book})";
    
    [XmlIgnore] 
    public HashSet<AccessoryMount> Mounts { get; set; } = new();
    
    [XmlIgnore]
    private ILogger? Logger { get; set; }
    
    [XmlIgnore]
    public static Dictionary<string, Accessory> AccessoriesDictionary { get; private set; } = new();

    public async Task CreateAsync(ILogger logger, ICreatable? baseObject = null)
    {
        const string accesoryMountStringPattern = @"([A-Z])\w+";
        await Task.Run(() =>
        {
            Logger = logger;
            
            var matches = Regex.Matches(MountString, accesoryMountStringPattern);
            foreach (Match match in matches)
            {
                if(Enum.TryParse<AccessoryMount>(match.ToString(), out var accessoryMount))
                    Mounts.Add(accessoryMount);
            }

            if (!AccessoriesDictionary.ContainsKey(Name))
                AccessoriesDictionary.Add(Name, this);
        });
    }

    public static Accessory GetAccessory(string name, ILogger logger)
    {
        return AccessoriesDictionary.GetValueByString(name, logger);
    }

    public static Accessory GetAccessory(Accessory accessoryWithName, ILogger logger)
    {
        return GetAccessory(accessoryWithName.Name, logger);
    }
}
