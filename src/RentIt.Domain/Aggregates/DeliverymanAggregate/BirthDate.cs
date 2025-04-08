using RentIt.Domain.Common;

namespace RentIt.Domain.Aggregates.DeliverymanAggregate;
public sealed class BirthDate : ValueObject
{
    public DateTime Value { get; }

    private BirthDate() { }

    public BirthDate(DateTime value)
    {
        if (value > DateTime.UtcNow)
            throw new ArgumentException("Birth date cannot be in the future.");

        Value = value;
    }

    public bool IsAdult(DateTime referenceDate)
    {
        return Value <= referenceDate.AddYears(-18);
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value.ToShortDateString();
}
