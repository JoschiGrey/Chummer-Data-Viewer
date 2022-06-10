namespace ChummerDataViewer.Interfaces;

public interface IHasDependency
{
    public bool CheckDependencies();

    public IReadOnlySet<Type> Dependencies {get;}
}