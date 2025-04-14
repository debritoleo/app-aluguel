using FluentAssertions;
using RentIt.Domain.Aggregates.MotorcycleAggregate;
using Xunit;

namespace RentIt.Tests.Unit;

public class MotorcycleDomainTests
{
    [Fact(DisplayName = "Motorcycle válido deve ser criado com sucesso")]
    public void Motorcycle_Valid_ShouldBeCreated()
    {
        var moto = new Motorcycle("id001", 2023, "Yamaha XTZ", new Plate("ABC-1234"));

        moto.Identifier.Should().Be("id001");
        moto.Model.Should().Be("Yamaha XTZ");
        moto.Year.Should().Be(2023);
        moto.Plate.Value.Should().Be("ABC-1234");
    }

    [Theory(DisplayName = "Motorcycle inválida deve lançar exceção")]
    [InlineData("", 2023, "XTZ")]
    [InlineData("id2", 1999, "XTZ")]
    [InlineData("id3", 2050, "XTZ")]
    [InlineData("id4", 2023, "")]
    public void Motorcycle_Invalid_ShouldThrow(string id, int year, string model)
    {
        var act = () => new Motorcycle(id, year, model, new Plate("XYZ-9999"));
        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Alterar placa deve funcionar corretamente")]
    public void Motorcycle_ChangePlate_ShouldUpdateValue()
    {
        var moto = new Motorcycle("id005", 2022, "CG Titan", new Plate("OLD-0001"));

        moto.ChangePlate(new Plate("NEW-1234"));

        moto.Plate.Value.Should().Be("NEW-1234");
    }

    [Fact(DisplayName = "Método IsFromYear2024 deve retornar verdadeiro somente para 2024")]
    public void Motorcycle_IsFromYear2024_ShouldWork()
    {
        var m1 = new Motorcycle("id006", 2024, "Biz", new Plate("BIZ-2024"));
        var m2 = new Motorcycle("id007", 2023, "Pop", new Plate("POP-2023"));

        m1.IsFromYear2024().Should().BeTrue();
        m2.IsFromYear2024().Should().BeFalse();
    }

    [Theory(DisplayName = "Plate inválida deve lançar exceção")]
    [InlineData(null)]
    [InlineData("")]
    [InlineData("TOO-LONG-PLATE")]
    public void Plate_Invalid_ShouldThrow(string input)
    {
        var act = () => new Plate(input);
        act.Should().Throw<ArgumentException>();
    }
}
