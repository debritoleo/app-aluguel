using FluentAssertions;
using RentIt.Domain.Aggregates.DeliverymanAggregate;
using Xunit;

namespace RentIt.Tests.Unit;

public class ValueObjectTests
{
    [Theory(DisplayName = "CNPJ válido deve ser aceito e formatado")]
    [InlineData("12.345.678/0001-99", "12345678000199")]
    [InlineData("12345678000199", "12345678000199")]
    public void Cnpj_ShouldBeNormalized(string input, string expected)
    {
        var cnpj = new Cnpj(input);
        cnpj.Value.Should().Be(expected);
    }

    [Theory(DisplayName = "CNPJ inválido deve lançar exceção")]
    [InlineData("")]
    [InlineData("12345678")]
    [InlineData("abc123")]
    public void Cnpj_Invalid_ShouldThrow(string input)
    {
        var act = () => new Cnpj(input);
        act.Should().Throw<ArgumentException>();
    }

    [Theory(DisplayName = "CNH válida deve ser aceita")]
    [InlineData("12345678900")]
    [InlineData("987654321")]
    public void CnhNumber_ShouldAcceptValidInput(string input)
    {
        var cnh = new CnhNumber(input);
        cnh.Value.Should().Be(input);
    }

    [Theory(DisplayName = "CNH inválida deve lançar exceção")]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("123456789012345678901")] 
    public void CnhNumber_Invalid_ShouldThrow(string input)
    {
        var act = () => new CnhNumber(input);
        act.Should().Throw<ArgumentException>();
    }

    [Theory(DisplayName = "BirthDate inválida (futuro) deve lançar exceção")]
    [InlineData("2100-01-01")]
    public void BirthDate_FutureDate_ShouldThrow(string birth)
    {
        var act = () => new BirthDate(DateTime.Parse(birth));
        act.Should().Throw<ArgumentException>()
            .WithMessage("Birth date cannot be in the future.");
    }
}
