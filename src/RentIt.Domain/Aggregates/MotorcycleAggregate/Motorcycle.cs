using RentIt.Domain.Common;

namespace RentIt.Domain.Aggregates.MotorcycleAggregate;

public class Motorcycle : BaseEntity, IAggregateRoot
{
    public string Identifier { get; private set; }
    public int Year { get; private set; }
    public string Model { get; private set; }
    public Plate Plate { get; private set; }

    private Motorcycle() { }

    public Motorcycle(string identifier, int year, string model, Plate plate)
    {
        Validate(identifier, year, model);
        Identifier = identifier;
        Year = year;
        Model = model;
        Plate = plate;
    }

    public void ChangePlate(Plate newPlate)
    {
        Plate = newPlate ?? throw new ArgumentNullException(nameof(newPlate));
    }

    private void Validate(string identifier, int year, string model)
    {
        if (string.IsNullOrWhiteSpace(identifier))
            throw new ArgumentException("Identifier is required.");

        if (year < 2000 || year > DateTime.UtcNow.Year + 1)
            throw new ArgumentException("Year must be between 2000 and next year.");

        if (string.IsNullOrWhiteSpace(model))
            throw new ArgumentException("Model is required.");
    }

    public bool IsFromYear2024() => Year == 2024;
}