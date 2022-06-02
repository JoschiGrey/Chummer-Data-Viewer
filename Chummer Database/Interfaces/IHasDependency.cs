namespace Chummer_Database.Interfaces;

public interface IHasDependency
{
    private static HashSet<Type> Dependencies { get; set; }

    public bool CheckDependencies();
}