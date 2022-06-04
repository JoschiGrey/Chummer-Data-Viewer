using System.Xml.Serialization;
using ChummerDataViewer.Extensions;
using ChummerDataViewer.Interfaces;

namespace ChummerDataViewer.Classes;


[XmlRoot("book")]
public class Book : IDisplayable, ICreatable {
    [XmlElement(ElementName="id")] 
    public Guid Id { get; set; }

    [XmlElement(ElementName = "name")] 
    public string Name { get; set; } = string.Empty;

    [XmlElement(ElementName = "code")] 
    public string Code { get; set; } = string.Empty;

    [XmlIgnore]
    public string DisplayName => Name;


    /// <summary>
    /// Key: BookCode Value:Book
    /// </summary>
    [XmlIgnore] 
    public static Dictionary<string, Book> BooksDictionary { get; set; } = new();

    public static Book GetBookByCode(string bookCode, ILogger logger)
    {
        return BooksDictionary.GetValueByString(bookCode, logger);
    }

    public Task CreateAsync(ILogger logger, ICreatable? baseObject = null)
    {
        BooksDictionary.Add(Code, this);
        return Task.CompletedTask;
    }
}
