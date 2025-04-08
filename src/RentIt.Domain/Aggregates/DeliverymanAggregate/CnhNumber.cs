using RentIt.Domain.Common;

namespace RentIt.Domain.Aggregates.DeliverymanAggregate;
public sealed class CnhNumber : ValueObject
{
    public string Value { get; }

    private CnhNumber() { }

    public CnhNumber(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("CNH number is required.");

        if (value.Length > 20)
            throw new ArgumentException("CNH number is too long.");

        Value = value;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
