using System.Text.Json.Serialization;

namespace RentIt.Application.Requests.Deliveryman;
public class UploadCnhRequest
{
    [JsonPropertyName("imagem_cnh")]
    public string CnhBase64 { get; set; } = string.Empty;
}
