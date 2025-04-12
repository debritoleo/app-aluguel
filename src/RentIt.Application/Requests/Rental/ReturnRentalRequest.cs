using System.Text.Json.Serialization;

namespace RentIt.Application.Requests.Rental;
public class ReturnRentalRequest
{
    [JsonPropertyName("data_devolucao")]
    public DateTime ReturnDate { get; set; }
}
