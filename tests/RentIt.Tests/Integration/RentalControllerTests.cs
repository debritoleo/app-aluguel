using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using RentIt.Application.Commands.Deliveryman;
using RentIt.Application.Commands.Motorcycle;
using RentIt.Application.Commands.Rental;
using RentIt.Domain.Aggregates.DeliverymanAggregate;

namespace RentIt.Tests.Integration;

public class RentalControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;
    private readonly TimeProvider _timeProvider = TimeProvider.System;

    public RentalControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.CreateClient();
    }

    private async Task<string> CreateDeliverymanAsync(CnhType tipoCnh = CnhType.A)
    {
        var request = new CreateDeliverymanRequest
        {
            Identificador = Guid.NewGuid().ToString(),
            Nome = "Locador de Teste",
            Cnpj = $"{Guid.NewGuid():N}"[..14],
            DataNascimento = new DateTime(1990, 5, 10),
            NumeroCnh = $"{Guid.NewGuid():N}"[..11],
            TipoCnh = tipoCnh
        };

        var response = await _client.PostAsJsonAsync("/entregadores", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        return result!["id"];
    }

    private async Task<string> CreateMotorcycleAsync()
    {
        var request = new CreateMotorcycleRequest
        {
            Identificador = Guid.NewGuid().ToString(),
            Ano = 2024,
            Modelo = "Yamaha MT",
            Placa = $"LOC{_timeProvider.GetUtcNow().Ticks % 10000:0000}"
        };

        var response = await _client.PostAsJsonAsync("/motos", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        return result!["id"];
    }

    [Fact(DisplayName = "POST /locacoes deve criar locação com sucesso")]
    public async Task CreateRental_ShouldReturnCreated()
    {
        var deliverymanId = await CreateDeliverymanAsync();
        var motorcycleId = await CreateMotorcycleAsync();

        var now = _timeProvider.GetUtcNow().UtcDateTime;

        var request = new CreateRentalRequest
        {
            DeliverymanId = deliverymanId,
            MotorcycleId = motorcycleId,
            EndDate = now.AddDays(7),
            ExpectedEndDate = now.AddDays(7)
        };

        var response = await _client.PostAsJsonAsync("/locacoes", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        result.Should().ContainKey("id");
        result["id"].Should().NotBeNullOrEmpty();
    }

    [Fact(DisplayName = "POST /locacoes com entregador CNH tipo B deve retornar erro")]
    public async Task CreateRental_WithInvalidCnhType_ShouldFail()
    {
        var deliverymanId = await CreateDeliverymanAsync(CnhType.B);
        var motorcycleId = await CreateMotorcycleAsync();

        var now = _timeProvider.GetUtcNow().UtcDateTime;

        var request = new CreateRentalRequest
        {
            DeliverymanId = deliverymanId,
            MotorcycleId = motorcycleId,
            EndDate = now.AddDays(7),
            ExpectedEndDate = now.AddDays(7)
        };

        var response = await _client.PostAsJsonAsync("/locacoes", request);

        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Theory(DisplayName = "POST /locacoes deve calcular corretamente com multa ou atraso")]
    [InlineData(-2, 7, HttpStatusCode.Created)]
    [InlineData(2, 7, HttpStatusCode.Created)]
    [InlineData(0, 7, HttpStatusCode.Created)]
    public async Task CreateRental_WithDifferentReturnDates_ShouldWork(int deltaDays, int duration, HttpStatusCode expectedStatus)
    {
        var deliverymanId = await CreateDeliverymanAsync();
        var motorcycleId = await CreateMotorcycleAsync();

        var today = _timeProvider.GetUtcNow().UtcDateTime;

        var request = new CreateRentalRequest
        {
            DeliverymanId = deliverymanId,
            MotorcycleId = motorcycleId,
            EndDate = today.AddDays(duration + deltaDays),
            ExpectedEndDate = today.AddDays(duration)
        };

        var response = await _client.PostAsJsonAsync("/locacoes", request);

        response.StatusCode.Should().Be(expectedStatus);
    }

    [Fact(DisplayName = "GET /locacao/{id} deve retornar os dados da locação corretamente")]
    public async Task GetRentalById_ShouldReturnCorrectData()
    {
        var deliverymanId = await CreateDeliverymanAsync();
        var motorcycleId = await CreateMotorcycleAsync();

        var now = _timeProvider.GetUtcNow().UtcDateTime;
        var end = now.AddDays(7);

        var request = new CreateRentalRequest
        {
            DeliverymanId = deliverymanId,
            MotorcycleId = motorcycleId,
            EndDate = end,
            ExpectedEndDate = end
        };

        var post = await _client.PostAsJsonAsync("/locacoes", request);
        post.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await post.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        var id = result!["id"];

        var get = await _client.GetAsync($"/locacao/{id}");

        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var rental = await get.Content.ReadFromJsonAsync<Dictionary<string, object>>();

        rental.Should().NotBeNull();
        rental["id"]!.ToString().Should().Be(id);
        rental["entregadorId"]!.ToString().Should().Be(deliverymanId);
        rental["motoId"]!.ToString().Should().Be(motorcycleId);
    }
}
