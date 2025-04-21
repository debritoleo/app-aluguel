using System.Text.Json.Serialization;

namespace RentIt.Application.Requests.Deliveryman;

public class CreateDeliverymanRequest
{
    [JsonPropertyName("identificador")]
    public string Identifier { get; set; } = string.Empty;

    [JsonPropertyName("nome")]
    public string Name { get; set; } = string.Empty;

    [JsonPropertyName("cnpj")]
    public string Cnpj { get; set; } = string.Empty;

    [JsonPropertyName("data_nascimento")]
    public DateTime BirthDate { get; set; }

    [JsonPropertyName("numero_cnh")]
    public string CnhNumber { get; set; } = string.Empty;

    [JsonPropertyName("tipo_cnh")]
    public string CnhType { get; set; } = string.Empty;
}
