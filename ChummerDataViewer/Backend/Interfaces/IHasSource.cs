using ChummerDataViewer.Backend.Classes;

namespace ChummerDataViewer.Backend.Interfaces;

public interface IHasSource
{
    public Source? Source { get; set; }
}