namespace Chummer_Database.Interfaces;


public interface ICreatable
{
    public Task CreateAsync(ILogger logger, ICreatable? baseObject = null);
}
