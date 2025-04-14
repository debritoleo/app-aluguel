using FluentAssertions;
using RentIt.Domain.Aggregates.RentalAggregate;
using RentIt.Domain.Aggregates.DeliverymanAggregate;
using Xunit;

namespace RentIt.Tests.Unit;

public class RentalDomainTests
{
    private readonly DateTime _now = new(2024, 1, 1);

    [Fact(DisplayName = "Rental deve calcular multa por devolução antecipada no plano de 7 dias")]
    public void Rental_ShouldCalculateEarlyReturnPenalty_Plan7Days()
    {
        var expectedEnd = _now.AddDays(8); 
        var actualReturn = _now.AddDays(6); 

        var rental = Rental.Create(
            motorcycleId: "mot001",
            deliverymanId: "ent001",
            endDate: actualReturn,
            expectedEndDate: expectedEnd,
            now: _now,
            tipoCnh: CnhType.A
        );

        var total = rental.CalculateTotalCost();

        total.Should().Be(5 * 30 + 2 * 30 * 0.2m);
    }

    [Fact(DisplayName = "Rental deve calcular custo extra por devolução atrasada")]
    public void Rental_ShouldCalculateLateReturnExtra()
    {
        var expectedEnd = _now.AddDays(16); 
        var actualReturn = _now.AddDays(18);

        var rental = Rental.Create(
            motorcycleId: "mot002",
            deliverymanId: "ent002",
            endDate: actualReturn,
            expectedEndDate: expectedEnd,
            now: _now,
            tipoCnh: CnhType.AB
        );

        var total = rental.CalculateTotalCost();

        total.Should().Be(15 * 28 + 2 * 50); 
    }

    [Fact(DisplayName = "Rental com devolução no prazo deve calcular valor base")]
    public void Rental_ShouldCalculateNormalReturn()
    {
        var expectedEnd = _now.AddDays(31);

        var rental = Rental.Create(
            motorcycleId: "mot003",
            deliverymanId: "ent003",
            endDate: expectedEnd,
            expectedEndDate: expectedEnd,
            now: _now,
            tipoCnh: CnhType.A
        );

        var total = rental.CalculateTotalCost();

        total.Should().Be(30 * 22);
    }

    [Fact(DisplayName = "Rental deve lançar exceção para CNH inválida")]
    public void Rental_ShouldThrowForInvalidCnh()
    {
        var act = () => Rental.Create(
            motorcycleId: "mot004",
            deliverymanId: "ent004",
            endDate: _now.AddDays(10),
            expectedEndDate: _now.AddDays(11),
            now: _now,
            tipoCnh: CnhType.B
        );

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Entregador não possui CNH do tipo A.");
    }
}
