using RentIt.Domain.Common;

namespace RentIt.Domain.Aggregates.RentalAggregate;

public class RentalPeriod : ValueObject
{
    public DateTime StartDate { get; }
    public DateTime EndDate { get; }
    public DateTime ExpectedEndDate { get; }

    private RentalPeriod() { }    

    public RentalPeriod(DateTime now, DateTime endDate, DateTime expectedEndDate)
    {
        StartDate = now.Date.AddDays(1);
        EndDate = endDate.Date;
        ExpectedEndDate = expectedEndDate.Date;

        if (ExpectedEndDate < StartDate)
            throw new ArgumentException("A data prevista de término deve ser após a data de início.");
    }

    public int TotalDays => (ExpectedEndDate - StartDate).Days;
    public int UsedDays(DateTime returnDate) => (returnDate.Date - StartDate).Days;
    public int ExtraDays(DateTime returnDate) => (returnDate.Date - ExpectedEndDate).Days;

    public bool Overlaps(RentalPeriod other)
        => StartDate < other.EndDate && other.StartDate < EndDate;

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return StartDate;
        yield return EndDate;
        yield return ExpectedEndDate;
    }
}