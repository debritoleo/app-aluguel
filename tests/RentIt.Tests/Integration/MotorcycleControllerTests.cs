using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using RentIt.Application.Commands.Motorcycle;
using Xunit;

namespace RentIt.Tests.Integration;

public class MotorcycleControllerTests : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client;

    public MotorcycleControllerTests(WebApplicationFactory<Program> factory)
    {
        _client = factory.WithWebHostBuilder(_ => { }).CreateClient();
    }

    [Fact(DisplayName = "POST /motos deve criar nova moto com sucesso")]
    public async Task PostMotorcycle_ShouldCreateSuccessfully()
    {
        var request = new CreateMotorcycleRequest
        {
            Identificador = Guid.NewGuid().ToString(),
            Ano = 2024,
            Modelo = "Yamaha Test",
            Placa = $"TEST{DateTime.UtcNow.Ticks % 10000:0000}"
        };

        var response = await _client.PostAsJsonAsync("/motos", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        result.Should().NotBeNull();
        result!["id"].Should().NotBeNullOrEmpty();
    }

    [Fact(DisplayName = "GET /motos/{id} deve retornar a moto corretamente")]
    public async Task GetMotorcycleById_ShouldReturnCorrectData()
    {
        var request = new CreateMotorcycleRequest
        {
            Identificador = Guid.NewGuid().ToString(),
            Ano = 2023,
            Modelo = "Honda Falcon",
            Placa = $"GET{DateTime.UtcNow.Ticks % 10000:0000}"
        };

        var postResponse = await _client.PostAsJsonAsync("/motos", request);
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var created = await postResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        var id = created!["id"];

        var getResponse = await _client.GetAsync($"/motos/{id}");

        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var moto = await getResponse.Content.ReadFromJsonAsync<Dictionary<string, object>>();
        moto!["id"].ToString().Should().Be(id);
        moto["identificador"].ToString().Should().Be(request.Identificador);
        moto["ano"].Should().Be(request.Ano);
        moto["modelo"].ToString().Should().Be(request.Modelo);
        moto["placa"].ToString().ToUpper().Should().Be(request.Placa.ToUpper());
    }

    [Fact(DisplayName = "POST /motos com placa duplicada deve retornar erro")]
    public async Task PostMotorcycle_WithDuplicatePlate_ShouldReturnBadRequest()
    {
        var plate = $"DUP{DateTime.UtcNow.Ticks % 10000:0000}";

        var request1 = new CreateMotorcycleRequest
        {
            Identificador = Guid.NewGuid().ToString(),
            Ano = 2022,
            Modelo = "CG Titan",
            Placa = plate
        };

        var request2 = new CreateMotorcycleRequest
        {
            Identificador = Guid.NewGuid().ToString(),
            Ano = 2023,
            Modelo = "CG Titan 2",
            Placa = plate
        };

        var firstResponse = await _client.PostAsJsonAsync("/motos", request1);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var secondResponse = await _client.PostAsJsonAsync("/motos", request2);
        secondResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var errors = await secondResponse.Content.ReadFromJsonAsync<Dictionary<string, string[]>>();
        errors.Should().NotBeNull();
    }
}
