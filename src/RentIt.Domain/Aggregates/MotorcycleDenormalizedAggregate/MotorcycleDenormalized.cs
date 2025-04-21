using RentIt.Domain.Common;

namespace RentIt.Domain.Aggregates.MotorcycleDenormalizedAggregate;

public class MotorcycleDenormalized : BaseEntity, IAggregateRoot
{
    private MotorcycleDenormalized()
    {
            
    }

    public MotorcycleDenormalized(string identifier, int year, string model, string plate, DateTime createdAt)
    {
        Identifier = identifier;
        Year = year;
        Model = model;
        Plate = plate;
        CreatedAt = createdAt;
    }

    public string Identifier { get; private set; } = default!;
    public int Year { get; private set; }
    public string Model { get; private set; } = default!;
    public string Plate { get; private set; } = default!;
    public DateTime CreatedAt { get; set; }
}
