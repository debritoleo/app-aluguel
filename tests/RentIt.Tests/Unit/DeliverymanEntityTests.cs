using FluentAssertions;
using RentIt.Domain.Aggregates.DeliverymanAggregate;

namespace RentIt.Tests.Unit;

public class DeliverymanEntityTests
{
    private readonly DateTime _referenceDate = new(2024, 1, 1);

    [Fact(DisplayName = "Deliveryman válido deve ser criado com sucesso")]
    public void Create_ValidDeliveryman_ShouldSucceed()
    {
        var act = () => new Deliveryman(
            identifier: "d1",
            name: "Rider One",
            cnpj: new Cnpj("12345678000199"),
            birthDate: new BirthDate(new DateTime(1990, 1, 1)),
            cnhNumber: new CnhNumber("98765432100"),
            cnhType: CnhType.A,
            referenceDate: _referenceDate
        );

        act.Should().NotThrow();
    }

    [Theory(DisplayName = "Deliveryman com dados inválidos deve lançar exceção")]
    [InlineData("", "João", "12345678000199", "2000-01-01", "12345678900", "A")]
    [InlineData("id1", "", "12345678000199", "2000-01-01", "12345678900", "A")]
    [InlineData("id2", "João", "12345678000199", "2010-01-01", "12345678900", "A")]
    public void Create_InvalidDeliveryman_ShouldThrow(string id, string name, string cnpj, string birth, string cnh, string cnhType)
    {
        var act = () => new Deliveryman(
            identifier: id,
            name: name,
            cnpj: new Cnpj(cnpj),
            birthDate: new BirthDate(DateTime.Parse(birth)),
            cnhNumber: new CnhNumber(cnh),
            cnhType: CnhType.FromName(cnhType),
            referenceDate: _referenceDate
        );

        act.Should().Throw<ArgumentException>();
    }

    [Fact(DisplayName = "Deliveryman.IsAdult deve funcionar com referência futura")]
    public void IsAdult_ShouldValidateAgainstReferenceDate()
    {
        var referenceDate = new DateTime(2024, 1, 1);
        var futureReference = referenceDate.AddYears(10);

        var deliveryman = new Deliveryman(
            identifier: "d2",
            name: "Rider Future",
            cnpj: new Cnpj("12345678000199"),
            birthDate: new BirthDate(new DateTime(2010, 1, 1)), 
            cnhNumber: new CnhNumber("12345678900"),
            cnhType: CnhType.A,
            referenceDate: futureReference 
        );

        deliveryman.IsAdult(futureReference).Should().BeTrue();
    }
}
