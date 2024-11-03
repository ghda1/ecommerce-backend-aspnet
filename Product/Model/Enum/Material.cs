// public enum for t-shirt meterial
using System.Text.Json.Serialization;

[JsonConverter(typeof(JsonStringEnumConverter))]
public enum Material
{
    Coton,
    Polyester,
    Blend,
    Linen,
    Crepe,
    Rayon,
    Silk
}