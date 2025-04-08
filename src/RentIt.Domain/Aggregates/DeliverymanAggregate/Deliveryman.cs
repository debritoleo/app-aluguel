using RentIt.Domain.Common;

namespace RentIt.Domain.Aggregates.DeliverymanAggregate;
public class Deliveryman : BaseEntity, IAggregateRoot
{
    public string Identifier { get; private set; }
    public string Name { get; private set; }
    public Cnpj Cnpj { get; private set; }
    public BirthDate BirthDate { get; private set; }
    public CnhNumber CnhNumber { get; private set; }
    public CnhType CnhType { get; private set; }

    private Deliveryman() { }

    public Deliveryman(
        string identifier,
        string name,
        Cnpj cnpj,
        BirthDate birthDate,
        CnhNumber cnhNumber,
        CnhType cnhType,
        DateTime referenceDate)
    {
        Validate(identifier, name, birthDate, referenceDate);
        Identifier = identifier;
        Name = name;
        Cnpj = cnpj;
        BirthDate = birthDate;
        CnhNumber = cnhNumber;
        CnhType = cnhType;
    }

    public bool CanRentMotorcycle() => CnhType == CnhType.A || CnhType == CnhType.AB;
    public bool IsAdult(DateTime referenceDate) => BirthDate.IsAdult(referenceDate);

    private void Validate(string identifier, string name, BirthDate birthDate, DateTime referenceDate)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            throw new ArgumentException("Identifier is required.");

        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Name is required.");

        if (!birthDate.IsAdult(referenceDate))
            throw new ArgumentException("Deliveryman must be at least 18 years old.");
    }
}
