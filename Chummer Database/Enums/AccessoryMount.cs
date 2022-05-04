using System.Xml.Serialization;

namespace Chummer_Database.Enums;

public enum AccessoryMount
{
    [XmlEnum("Stock")]
    Stock,
    
    [XmlEnum("Side")]
    Side,
    
    [XmlEnum("Top")]
    Top,
    
    [XmlEnum("Under")]
    Under,
    
    [XmlEnum("Barrel")]
    Barrel
}