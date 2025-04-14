namespace RentIt.IntegrationEvents.Motorcycle;

public class MotorcycleCreatedEvent
{
    public string Id { get; set; } = default!;
    public string Identifier { get; set; } = default!;
    public int Year { get; set; }
    public string Model { get; set; } = default!;
    public string Plate { get; set; } = default!;
}