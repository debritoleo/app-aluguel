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
        // StartDate = 2024-01-02, ExpectedEndDate = 2024-01-08
        var expectedEnd = _now.AddDays(8);
        var actualReturn = _now.AddDays(6); // devolveu em 2024-01-07

        var rental = Rental.Create(
            motorcycleId: "mot001",
            deliverymanId: "ent001",
            endDate: actualReturn,
            expectedEndDate: expectedEnd,
            now: _now,
            tipoCnh: CnhType.A
        );

        var total = rental.CalculateTotalCost(actualReturn);

        // 7 dias * R$30 = R$210
        // 2 dias não usados * 30 * 20% = R$12
        total.Should().Be(210 + 12);
    }

    [Fact(DisplayName = "Rental deve calcular custo extra por devolução atrasada")]
    public void Rental_ShouldCalculateLateReturnExtra()
    {
        // StartDate = 2024-01-02, ExpectedEndDate = 2024-01-17
        var expectedEnd = _now.AddDays(16); // 15 dias de locação
        var actualReturn = _now.AddDays(18); // devolveu em 2024-01-19 (2 dias de atraso)

        var rental = Rental.Create(
            motorcycleId: "mot002",
            deliverymanId: "ent002",
            endDate: actualReturn,
            expectedEndDate: expectedEnd,
            now: _now,
            tipoCnh: CnhType.AB
        );

        var total = rental.CalculateTotalCost(actualReturn);

        // 15 dias * R$28 = R$420
        // 2 dias extra * R$50 = R$100
        total.Should().Be(420 + 100);
    }

    [Fact(DisplayName = "Rental com devolução no prazo deve calcular valor base")]
    public void Rental_ShouldCalculateNormalReturn()
    {
        // StartDate = 2024-01-02, ExpectedEndDate = 2024-02-01
        var expectedEnd = _now.AddDays(31); // 30 dias de locação

        var rental = Rental.Create(
            motorcycleId: "mot003",
            deliverymanId: "ent003",
            endDate: expectedEnd,
            expectedEndDate: expectedEnd,
            now: _now,
            tipoCnh: CnhType.A
        );

        var total = rental.CalculateTotalCost();

        // 30 dias * R$22 = R$660
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
            tipoCnh: CnhType.B // ❌ inválido
        );

        act.Should().Throw<InvalidOperationException>()
            .WithMessage("Entregador não possui CNH do tipo A.");
    }
}
