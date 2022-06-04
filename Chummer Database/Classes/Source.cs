using Chummer_Database.Interfaces;

namespace Chummer_Database.Classes;

public record Source : IDisplayable
{
    public int Page { get; }
    public Book Book { get; }
    public string DisplayName { get; }
    
    private ILogger Logger { get; }

    public override string ToString()
    {
        return DisplayName;
    }

    public Source(string bookCode, int page, ILogger logger)
    {
        Logger = logger;
        Book = Book.GetBookByCode(bookCode, Logger);
        Page = page;
        DisplayName = $"p.{Page} ({Book.Code})";
    }
}