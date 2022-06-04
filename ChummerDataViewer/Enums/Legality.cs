using System.Text.Json.Serialization;

namespace ChummerDataViewer.Enums;

[JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
public enum Legality
{
    Restricted,
    Forbidden,
    Unrestricted
}