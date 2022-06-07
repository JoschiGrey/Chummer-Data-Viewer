using ChummerDataViewer.Classes;

namespace ChummerDataViewer.Interfaces;

public interface IHasSource
{
    public Source Source { get; set; }
}