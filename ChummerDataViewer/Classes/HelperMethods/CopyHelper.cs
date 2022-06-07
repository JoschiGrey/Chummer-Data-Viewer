using System.Xml.Serialization;

namespace ChummerDataViewer.Classes.HelperMethods;

public class CopyHelper
{
    public static T CreateDeepCopy<T>(T obj)
    {
        ArgumentNullException.ThrowIfNull(obj, nameof(obj));
        using var ms = new MemoryStream();
        var serializer = new XmlSerializer(obj.GetType());
        serializer.Serialize(ms, obj);
        
        ms.Seek(0, SeekOrigin.Begin);

        var copiedObject = serializer.Deserialize(ms);
        if (copiedObject is null)
            throw new NullReferenceException(nameof(copiedObject));

        return (T) copiedObject;
    }
}