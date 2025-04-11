using RentIt.Domain.Aggregates.DeliverymanAggregate;
using System.Text.Json.Serialization;

namespace RentIt.Application.Commands.Deliveryman;

public class CreateDeliverymanRequest
{
    public string Identificador { get; set; } = string.Empty;

    public string Nome { get; set; } = string.Empty;

    public string Cnpj { get; set; } = string.Empty;

    [JsonPropertyName("data_nascimento")]
    public DateTime DataNascimento { get; set; }

    [JsonPropertyName("numero_cnh")]
    public string NumeroCnh { get; set; } = string.Empty;

    [JsonPropertyName("tipo_cnh")]
    public CnhType TipoCnh { get; set; }
}
