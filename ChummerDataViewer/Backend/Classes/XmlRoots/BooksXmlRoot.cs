using System.Xml.Serialization;
using ChummerDataViewer.Backend.Interfaces;

namespace ChummerDataViewer.Backend.Classes.XmlRoots;

[XmlRoot("chummer")]
public class BooksXmlRoot : ICreatable
{
    [XmlArray("books")]
    [XmlArrayItem("book")]
    public List<Book> Books { get; set; } = new();

    [XmlIgnore]
    private ILogger? Logger { get; set; }

    public async Task CreateAsync(ILogger logger, ICreatable? baseObject)
    {
        Logger = logger;

        var taskList = new List<Task>();
        foreach (var book in Books)
        {
            taskList.Add(book.CreateAsync(logger));
        }
        
        await Task.WhenAll(taskList).ConfigureAwait(false);
        XmlLoader.CreatedXml.Add(GetType());
        logger.LogInformation("Created {Type}", GetType().Name);

    }
}