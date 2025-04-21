using System.Net;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RentIt.Application.Requests.Deliveryman;
using RentIt.Domain.Aggregates.DeliverymanAggregate;

namespace RentIt.Tests.Integration;

public class DeliverymanControllerTests : BaseIntegrationTest
{
    public DeliverymanControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
        
    }

    [Fact(DisplayName = "POST /entregadores deve cadastrar entregador com sucesso")]
    public async Task CreateDeliveryman_ShouldReturnCreated()
    {
        var request = new CreateDeliverymanRequest
        {
            Identifier = Guid.NewGuid().ToString(),
            Name = "João da Entrega",
            Cnpj = "12345678000199",
            BirthDate = new DateTime(1990, 5, 10),
            CnhNumber = "12345678901",
            CnhType = CnhType.A.Name
        };

        var response = await Client.PostAsJsonAsync("/entregadores", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact(DisplayName = "POST /entregadores com CNPJ duplicado deve retornar erro")]
    public async Task CreateDeliveryman_WithDuplicateCnpj_ShouldReturnBadRequest()
    {
        var cnpj = "11222333000199";

        var request = new CreateDeliverymanRequest
        {
            Identifier = Guid.NewGuid().ToString(),
            Name = "Rider One",
            Cnpj = cnpj,
            BirthDate = new DateTime(1990, 5, 10),
            CnhNumber = "98765432100",
            CnhType = CnhType.AB.Name
        };

        var first = await Client.PostAsJsonAsync("/entregadores", request);
        first.StatusCode.Should().Be(HttpStatusCode.Created);

        request.Identifier = Guid.NewGuid().ToString();
        request.CnhNumber = "98765432101";

        var second = await Client.PostAsJsonAsync("/entregadores", request);
        second.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await second.Content.ReadAsStringAsync();
        body.Should().Contain("mensagem").And.Contain("Dados inválidos");
    }

    [Fact(DisplayName = "POST /entregadores/{id}/cnh com base64 válido deve retornar sucesso")]
    public async Task UploadCnh_Base64Valido_DeveRetornarCreated()
    {
        var id = Guid.NewGuid().ToString();

        var payload = new
        {
            imagem_cnh = "iVBORw0KGgoAAAANSUhEUgAAAAEAAAABCAYAAAAfFcSJAAAACklEQVR42mMAAQAABQABDQottAAAAABJRU5ErkJggg=="
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await Client.PostAsync($"/entregadores/{id}/cnh", content);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact(DisplayName = "POST /entregadores/{id}/cnh com base64 inválido deve retornar erro")]
    public async Task UploadCnh_Base64Invalido_DeveRetornarBadRequest()
    {
        var id = Guid.NewGuid().ToString();

        var payload = new
        {
            imagem_cnh = "BASE64INVALIDA123=="
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await Client.PostAsync($"/entregadores/{id}/cnh", content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var json = await response.Content.ReadAsStringAsync();
        json.Should().Contain("mensagem").And.Contain("Dados inválidos");
    }

    [Fact(DisplayName = "POST /entregadores/{id}/cnh com payload vazio deve retornar erro")]
    public async Task UploadCnh_Vazio_DeveRetornarBadRequest()
    {
        var id = Guid.NewGuid().ToString();

        var payload = new
        {
            imagem_cnh = ""
        };

        var content = new StringContent(JsonSerializer.Serialize(payload), Encoding.UTF8, "application/json");

        var response = await Client.PostAsync($"/entregadores/{id}/cnh", content);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var json = await response.Content.ReadAsStringAsync();
        json.Should().Contain("mensagem").And.Contain("Dados inválidos");
    }
}
