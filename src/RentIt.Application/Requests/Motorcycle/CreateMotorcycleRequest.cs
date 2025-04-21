using System.Text.Json.Serialization;

namespace RentIt.Application.Commands.Motorcycle;

public class CreateMotorcycleRequest
{
    [JsonPropertyName("identificador")]
    public string Identifier { get; set; } = string.Empty;

    [JsonPropertyName("ano")]
    public int Year { get; set; }

    [JsonPropertyName("modelo")]
    public string Model { get; set; } = string.Empty;

    [JsonPropertyName("placa")]
    public string Plate { get; set; } = string.Empty;
}

