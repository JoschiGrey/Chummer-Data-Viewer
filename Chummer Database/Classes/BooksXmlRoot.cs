using System.Xml.Serialization;
using Chummer_Database.Interfaces;

namespace Chummer_Database.Classes;

[XmlRoot("chummer")]
public class BooksXmlRoot : ICreatable
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
        logger.LogInformation("Creating {Type}", GetType().Name);
        Logger = logger;

        BooksDictionary = Books.ToDictionary(book => book.Code);
        
        return true;
    }
    
    public async Task<ICreatable> CreateAsync(ILogger logger)
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