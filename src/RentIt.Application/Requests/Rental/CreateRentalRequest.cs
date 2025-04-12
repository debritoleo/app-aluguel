using System.Text.Json.Serialization;

namespace RentIt.Application.Commands.Rental;
public class CreateRentalRequest
{
    [JsonPropertyName("id_entregador")]
    public string DeliverymanId { get; set; } = string.Empty;

    [JsonPropertyName("id_moto")]
    public string MotorcycleId { get; set; } = string.Empty;

    [JsonPropertyName("data_termino")]
    public DateTime EndDate { get; set; }

    [JsonPropertyName("data_prevista_termino")]
    public DateTime ExpectedEndDate { get; set; }
}
