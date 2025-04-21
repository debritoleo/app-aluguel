using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc.Testing;
using RentIt.Application.Commands.Motorcycle;
using RentIt.Application.ViewModels.Motorcycle;

namespace RentIt.Tests.Integration;

public class MotorcycleControllerTests : BaseIntegrationTest
{
    public MotorcycleControllerTests(WebApplicationFactory<Program> factory) : base(factory)
    {
    }

    [Fact(DisplayName = "POST /motos deve criar nova moto com sucesso")]
    public async Task PostMotorcycle_ShouldCreateSuccessfully()
    {
        var request = new CreateMotorcycleRequest
        {
            Identifier = Guid.NewGuid().ToString(),
            Year = 2024,
            Model = "Yamaha Test",
            Plate = $"TEST{DateTime.UtcNow.Ticks % 10000:0000}"
        };

        var response = await Client.PostAsJsonAsync("/motos", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Fact(DisplayName = "GET /motos/{id} deve retornar a moto corretamente")]
    public async Task GetMotorcycleById_ShouldReturnCorrectData()
    {
        var request = new CreateMotorcycleRequest
        {
            Identifier = Guid.NewGuid().ToString(),
            Year = 2023,
            Model = "Honda Falcon",
            Plate = $"GET{DateTime.UtcNow.Ticks % 10000:0000}"
        };

        var postResponse = await Client.PostAsJsonAsync("/motos", request);
        postResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var allMotosResponse = await Client.GetAsync("/motos");
        var allMotos = await allMotosResponse.Content.ReadFromJsonAsync<List<MotorcycleViewModel>>();
        var moto = allMotos!.First(m => m.Identifier == request.Identifier);

        var getResponse = await Client.GetAsync($"/motos/{moto.Id}");
        getResponse.StatusCode.Should().Be(HttpStatusCode.OK);

        var returned = await getResponse.Content.ReadFromJsonAsync<MotorcycleViewModel>();

        returned!.Id.Should().Be(moto.Id);
        returned.Identifier.Should().Be(request.Identifier);
        returned.Year.Should().Be(request.Year);
        returned.Model.Should().Be(request.Model);
        returned.Plate.Should().Be(request.Plate);
    }

    [Fact(DisplayName = "POST /motos com placa duplicada deve retornar erro")]
    public async Task PostMotorcycle_WithDuplicatePlate_ShouldReturnBadRequest()
    {
        var plate = $"DUP{DateTime.UtcNow.Ticks % 10000:0000}";

        var request1 = new CreateMotorcycleRequest
        {
            Identifier = Guid.NewGuid().ToString(),
            Year = 2022,
            Model = "CG Titan",
            Plate = plate
        };

        var request2 = new CreateMotorcycleRequest
        {
            Identifier = Guid.NewGuid().ToString(),
            Year = 2023,
            Model = "CG Titan 2",
            Plate = plate
        };

        var firstResponse = await Client.PostAsJsonAsync("/motos", request1);
        firstResponse.StatusCode.Should().Be(HttpStatusCode.Created);

        var secondResponse = await Client.PostAsJsonAsync("/motos", request2);
        secondResponse.StatusCode.Should().Be(HttpStatusCode.BadRequest);

        var body = await secondResponse.Content.ReadAsStringAsync();
        body.Should().Contain("mensagem").And.Contain("Dados inválidos");
    }

    [Fact(DisplayName = "GET /motos/{id} com id inexistente deve retornar NotFound")]
    public async Task GetMotorcycleById_Nonexistent_ShouldReturnNotFound()
    {
        var response = await Client.GetAsync($"/motos/{Guid.NewGuid()}");

        response.StatusCode.Should().Be(HttpStatusCode.NotFound);

        var body = await response.Content.ReadAsStringAsync();
        body.Should().Contain("Moto não encontrada");
    }
}
