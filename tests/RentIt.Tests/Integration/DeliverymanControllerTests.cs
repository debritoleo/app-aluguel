using System.Net;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using RentIt.Application.Commands.Deliveryman;
using RentIt.Domain.Aggregates.DeliverymanAggregate;

namespace RentIt.Tests.Integration;

public class DeliverymanControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public DeliverymanControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    [Fact(DisplayName = "POST /entregadores deve cadastrar entregador com sucesso")]
    public async Task CreateDeliveryman_ShouldReturnCreated()
    {
        var request = new CreateDeliverymanRequest
        {
            Identificador = Guid.NewGuid().ToString(),
            Nome = "João da Entrega",
            Cnpj = "12345678000199",
            DataNascimento = new DateTime(1990, 5, 10),
            NumeroCnh = "12345678901",
            TipoCnh = CnhType.A
        };

        var response = await _client.PostAsJsonAsync("/entregadores", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        result.Should().ContainKey("id");
        result!["id"].Should().NotBeNullOrEmpty();
    }

    [Fact(DisplayName = "POST /entregadores com CNPJ duplicado deve retornar erro")]
    public async Task CreateDeliveryman_WithDuplicateCnpj_ShouldReturnBadRequest()
    {
        var cnpj = "11222333000199";

        var request = new CreateDeliverymanRequest
        {
            Identificador = Guid.NewGuid().ToString(),
            Nome = "Rider One",
            Cnpj = cnpj,
            DataNascimento = new DateTime(1990, 5, 10),
            NumeroCnh = "98765432100",
            TipoCnh = CnhType.AB
        };

        var first = await _client.PostAsJsonAsync("/entregadores", request);
        first.StatusCode.Should().Be(HttpStatusCode.Created);

        request.Identificador = Guid.NewGuid().ToString();
        request.NumeroCnh = "98765432101";

        var second = await _client.PostAsJsonAsync("/entregadores", request);
        second.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact(DisplayName = "POST /entregadores/{id}/cnh deve aceitar upload de PNG válido")]
    public async Task UploadCnh_ShouldAcceptPngFile()
    {
        var deliveryman = new CreateDeliverymanRequest
        {
            Identificador = Guid.NewGuid().ToString(),
            Nome = "Carlos Entregador",
            Cnpj = "78945612000100",
            DataNascimento = new DateTime(1985, 2, 20),
            NumeroCnh = "32165498700",
            TipoCnh = CnhType.A
        };

        var createResponse = await _client.PostAsJsonAsync("/entregadores", deliveryman);
        var result = await createResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        var id = result!["id"];

        var imageContent = new ByteArrayContent(new byte[] { 0x89, 0x50, 0x4E, 0x47 });
        imageContent.Headers.ContentType = new MediaTypeHeaderValue("image/png");

        using var form = new MultipartFormDataContent
        {
            { imageContent, "arquivo", "cnh.png" }
        };

        var uploadResponse = await _client.PostAsync($"/entregadores/{id}/cnh", form);

        uploadResponse.StatusCode.Should().Be(HttpStatusCode.NoContent);
    }

    [Fact(DisplayName = "POST /entregadores/{id}/cnh com formato inválido deve retornar erro")]
    public async Task UploadCnh_InvalidFormat_ShouldReturnBadRequest()
    {
        var deliveryman = new CreateDeliverymanRequest
        {
            Identificador = Guid.NewGuid().ToString(),
            Nome = "Carlos JPG",
            Cnpj = "78945612000188",
            DataNascimento = new DateTime(1985, 2, 20),
            NumeroCnh = "32165498711",
            TipoCnh = CnhType.AB
        };

        var createResponse = await _client.PostAsJsonAsync("/entregadores", deliveryman);
        var result = await createResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        var id = result!["id"];

        var invalidContent = new ByteArrayContent(new byte[] { 0xFF, 0xD8 }); // assinatura JPG
        invalidContent.Headers.ContentType = new MediaTypeHeaderValue("image/jpeg");

        using var form = new MultipartFormDataContent
        {
            { invalidContent, "arquivo", "foto.jpg" }
        };

        var response = await _client.PostAsync($"/entregadores/{id}/cnh", form);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}
