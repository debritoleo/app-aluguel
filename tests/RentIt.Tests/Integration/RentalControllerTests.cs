using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RentIt.Application.Commands.Motorcycle;
using RentIt.Application.Commands.Rental;
using RentIt.Application.Requests.Deliveryman;
using RentIt.Domain.Aggregates.DeliverymanAggregate;
using RentIt.Application.ViewModels.Rental;

namespace RentIt.Tests.Integration;

public class RentalControllerTests : BaseIntegrationTest
{
    private readonly TimeProvider _timeProvider = TimeProvider.System;

    public RentalControllerTests(WebApplicationFactory<Program> factory) : base(factory) { }

    private async Task<string> CreateDeliverymanAsync(string tipoCnh = "A")
    {
        var request = new CreateDeliverymanRequest
        {
            Identifier = Guid.NewGuid().ToString(),
            Name = "Locador de Teste",
            Cnpj = $"{DateTime.UtcNow.Ticks % 100000000000000:00000000000000}",
            BirthDate = new DateTime(1990, 5, 10),
            CnhNumber = $"{Guid.NewGuid():N}"[..11],
            CnhType = tipoCnh
        };

        var response = await Client.PostAsJsonAsync("/entregadores", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var json = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        return json!["id"];
    }

    private async Task<string> CreateMotorcycleAsync()
    {
        var request = new CreateMotorcycleRequest
        {
            Identifier = Guid.NewGuid().ToString(),
            Year = 2024,
            Model = "Yamaha MT",
            Plate = $"LOC{_timeProvider.GetUtcNow().Ticks % 10000:0000}"
        };

        var response = await Client.PostAsJsonAsync("/motos", request);
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
            EndDate = now.AddDays(8),
            ExpectedEndDate = now.AddDays(8)
        };

        var response = await Client.PostAsJsonAsync("/locacoes", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact(DisplayName = "POST /locacoes com CNH tipo B deve retornar erro")]
    public async Task CreateRental_WithInvalidCnhType_ShouldReturnBadRequest()
    {
        var deliverymanId = await CreateDeliverymanAsync(CnhType.B.Name);
        var motorcycleId = await CreateMotorcycleAsync();
        var now = _timeProvider.GetUtcNow().UtcDateTime;

        var request = new CreateRentalRequest
        {
            DeliverymanId = deliverymanId,
            MotorcycleId = motorcycleId,
            EndDate = now.AddDays(8),
            ExpectedEndDate = now.AddDays(8)
        };

        var response = await Client.PostAsJsonAsync("/locacoes", request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("mensagem").And.Contain("Dados inválidos");
    }

    [Theory(DisplayName = "POST /locacoes com períodos válidos deve retornar sucesso")]
    [InlineData(7)]
    [InlineData(15)]
    [InlineData(30)]
    [InlineData(45)]
    [InlineData(50)]
    public async Task CreateRental_WithValidPlanDays_ShouldSucceed(int duration)
    {
        var deliverymanId = await CreateDeliverymanAsync();
        var motorcycleId = await CreateMotorcycleAsync();
        var today = _timeProvider.GetUtcNow().UtcDateTime;

        var request = new CreateRentalRequest
        {
            DeliverymanId = deliverymanId,
            MotorcycleId = motorcycleId,
            EndDate = today.AddDays(duration + 1),
            ExpectedEndDate = today.AddDays(duration + 1)
        };

        var response = await Client.PostAsJsonAsync("/locacoes", request);
        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Theory(DisplayName = "POST /locacoes com períodos inválidos deve retornar erro")]
    [InlineData(6)]
    [InlineData(8)]
    [InlineData(10)]
    [InlineData(14)]
    public async Task CreateRental_WithInvalidPlanDays_ShouldFail(int duration)
    {
        var deliverymanId = await CreateDeliverymanAsync();
        var motorcycleId = await CreateMotorcycleAsync();
        var today = _timeProvider.GetUtcNow().UtcDateTime;

        var request = new CreateRentalRequest
        {
            DeliverymanId = deliverymanId,
            MotorcycleId = motorcycleId,
            EndDate = today.AddDays(duration + 1),
            ExpectedEndDate = today.AddDays(duration + 1)
        };

        var response = await Client.PostAsJsonAsync("/locacoes", request);
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("mensagem").And.Contain("Dados inválidos");
    }

    [Fact(DisplayName = "GET /locacao/{id} deve retornar dados corretos")]
    public async Task GetRentalById_ShouldReturnCorrectData()
    {
        var deliverymanId = await CreateDeliverymanAsync();
        var motorcycleId = await CreateMotorcycleAsync();
        var now = _timeProvider.GetUtcNow().UtcDateTime;

        var request = new CreateRentalRequest
        {
            DeliverymanId = deliverymanId,
            MotorcycleId = motorcycleId,
            EndDate = now.AddDays(8),
            ExpectedEndDate = now.AddDays(8)
        };

        var post = await Client.PostAsJsonAsync("/locacoes", request);
        post.StatusCode.Should().Be(HttpStatusCode.Created);

        var result = await post.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        var rentalId = result!["id"];

        var get = await Client.GetAsync($"/locacoes/{rentalId}");
        get.StatusCode.Should().Be(HttpStatusCode.OK);

        var rental = await get.Content.ReadFromJsonAsync<RentalViewModel>();
        rental.Should().NotBeNull();
        rental!.Id.Should().Be(rentalId);
        rental.DeliverymanId.Should().Be(deliverymanId);
        rental.MotorcycleId.Should().Be(motorcycleId);
    }

    [Fact(DisplayName = "GET /locacoes/{id} com id inexistente deve retornar 404")]
    public async Task GetRentalById_NotFound_ShouldReturnNotFound()
    {
        var get = await Client.GetAsync($"/locacoes/{Guid.NewGuid()}");
        get.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var json = await get.Content.ReadAsStringAsync();
        json.Should().Contain("Locação não encontrada");
    }
}
