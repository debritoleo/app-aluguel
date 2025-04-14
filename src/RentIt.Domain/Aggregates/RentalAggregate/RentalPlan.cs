using RentIt.Domain.Common;

namespace RentIt.Domain.Aggregates.RentalAggregate;
public class RentalPlan : ValueObject
{
    public int Days { get; }
    public decimal DailyRate { get; }
    public decimal EarlyReturnPenaltyPercentage { get; }
    public decimal LateFeePerDay { get; } = 50.00m;

    private RentalPlan() { }

    private RentalPlan(int days, decimal dailyRate, decimal earlyPenalty)
    {
        Days = days;
        DailyRate = dailyRate;
        EarlyReturnPenaltyPercentage = earlyPenalty;
    }

    public static RentalPlan CreateFromDuration(int totalDays)
    {
        return totalDays switch
        {
            7 => new RentalPlan(7, 30m, 0.20m),
            15 => new RentalPlan(15, 28m, 0.40m),
            30 => new RentalPlan(30, 22m, 0.00m),
            45 => new RentalPlan(45, 20m, 0.00m),
            50 => new RentalPlan(50, 18m, 0.00m),
            _ => throw new ArgumentException("Plano inválido para o período informado.")
        };
    }

    public decimal CalculateTotalValue() => Days * DailyRate;

    public decimal CalculatePenaltyForEarlyReturn(int daysUsed)
    {
        var unusedDays = Days - daysUsed;
        return unusedDays > 0
            ? unusedDays * DailyRate * EarlyReturnPenaltyPercentage
            : 0;
    }

    public decimal CalculateLateFee(int extraDays)
    {
        return extraDays * LateFeePerDay;
    }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return Days;
        yield return DailyRate;
        yield return EarlyReturnPenaltyPercentage;
    }
}
