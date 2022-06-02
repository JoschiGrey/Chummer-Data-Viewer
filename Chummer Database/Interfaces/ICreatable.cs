namespace Chummer_Database.Interfaces;


public interface ICreatable
{
    public Task<ICreatable> CreateAsync(ILogger logger);
}
