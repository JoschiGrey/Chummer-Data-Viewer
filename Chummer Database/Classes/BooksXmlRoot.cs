using System.Xml.Serialization;

namespace Chummer_Database.Classes;

[XmlRoot("chummer")]
public class BooksXmlRoot
{
    [XmlArray("books")]
    [XmlArrayItem("book")]
    public HashSet<Book> Books { get; set; } = new();

    [XmlIgnore]
    public Dictionary<string, Book> BooksDictionary { get; set; } = new();

    [XmlIgnore]
    private ILogger? Logger { get; set; }

    public bool Create(ILogger logger)
    {
        Logger = logger;

        BooksDictionary = Books.ToDictionary(book => book.Code);
        
        return true;
    }
}