using RentIt.Domain.Common;

namespace RentIt.Domain.Aggregates.MotorcycleAggregate;

public sealed class Plate : ValueObject
{
    public string Value { get; }

    private Plate() { }

    public Plate(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("A placa deve ser informada.");

        if (value.Length > 10)
            throw new ArgumentException("A placa não pode exceder 10 caracteres.");

        Value = value.ToUpperInvariant();
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}