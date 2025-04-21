using RentIt.Domain.Common;

namespace RentIt.Domain.Aggregates.DeliverymanAggregate;

public sealed class CnhType : Enumeration
{
    private static readonly CnhType None = new(0, "N/A");
    public static readonly CnhType A = new(1, "A");
    public static readonly CnhType B = new(2, "B");
    public static readonly CnhType AB = new(3, "AB");

    private CnhType(int id, string name) : base(id, name)
    {
    }

    public static CnhType FromId(int id)
    {
        var cnhType = GetAll<CnhType>()
                         .FirstOrDefault(x => x.Id == id);

        return cnhType ?? throw new ArgumentException($"Tipo de CNH com id '{id}' é inválido.", nameof(id));
    }

    public static CnhType FromName(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("O nome do tipo de CNH não pode ser nulo ou vazio.", nameof(name));

        var cnhType = GetAll<CnhType>()
                        .FirstOrDefault(ct => string.Equals(ct.Name, name, StringComparison.OrdinalIgnoreCase));
        
        return cnhType ?? throw new ArgumentException($"Tipo de CNH '{name}' é inválido.", nameof(name));
    }
}
