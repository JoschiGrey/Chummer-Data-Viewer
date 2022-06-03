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

    public static Book GetBookByCode(string bookCode)
    {
        if (XmlLoader.BooksXmlData is null)
            throw new ArgumentNullException(nameof(XmlLoader.BooksXmlData));
        
        return XmlLoader.BooksXmlData.BooksDictionary[bookCode];
    }
}
