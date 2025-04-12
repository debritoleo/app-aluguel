using RentIt.Domain.Aggregates.DeliverymanAggregate;
using RentIt.Domain.Common;

namespace RentIt.Domain.Aggregates.RentalAggregate;
public class Rental : BaseEntity, IAggregateRoot
{
    public string MotorcycleId { get; private set; }
    public string DeliverymanId { get; private set; }
    public RentalPeriod Period { get; private set; }
    public RentalPlan Plan { get; private set; }

    private Rental() { }

    private Rental(string motorcycleId, string deliverymanId, RentalPeriod period)
    {
        MotorcycleId = motorcycleId;
        DeliverymanId = deliverymanId;
        Period = period;
        Plan = RentalPlan.CreateFromDuration(period.TotalDays);
    }

    public static Rental Create(string motorcycleId, string deliverymanId, DateTime endDate, DateTime expectedEndDate, DateTime now, CnhType tipoCnh)
    {
        if (tipoCnh != CnhType.A && tipoCnh != CnhType.AB)
            throw new InvalidOperationException("Entregador não possui CNH do tipo A.");

        var period = new RentalPeriod(now, endDate, expectedEndDate);

        return new Rental(motorcycleId, deliverymanId, period);
    }

    public decimal CalculateTotalCost(DateTime? actualReturnDate = null)
    {
        var returnDate = actualReturnDate?.Date ?? Period.ExpectedEndDate;

        if (returnDate < Period.ExpectedEndDate)
        {
            var daysUsed = Period.UsedDays(returnDate);
            return Plan.CalculateTotalValue() + Plan.CalculatePenaltyForEarlyReturn(daysUsed);
        }
        else if (returnDate > Period.ExpectedEndDate)
        {
            var extraDays = Period.ExtraDays(returnDate);
            return Plan.CalculateTotalValue() + Plan.CalculateLateFee(extraDays);
        }

        return Plan.CalculateTotalValue();
    }
}