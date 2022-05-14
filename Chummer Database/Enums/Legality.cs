using System.Text.Json.Serialization;

namespace Chummer_Database.Enums;

[JsonConverter(typeof(System.Text.Json.Serialization.JsonStringEnumConverter))]
public enum Legality
{
    Restricted,
    Forbidden,
    Unrestricted
}