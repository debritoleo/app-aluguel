using RentIt.Domain.Common;
using System.Text.RegularExpressions;

namespace RentIt.Domain.Aggregates.DeliverymanAggregate;
public sealed class Cnpj : ValueObject
{
    public string Value { get; }

    private Cnpj() { }

    public Cnpj(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("CNPJ is required.");

        var cleaned = Regex.Replace(value, @"[^\d]", "");

        if (cleaned.Length != 14)
            throw new ArgumentException("CNPJ must have 14 digits.");

        Value = cleaned;
    }

    protected override IEnumerable<object?> GetEqualityComponents()
    {
        yield return Value;
    }

    public override string ToString() => Value;
}
