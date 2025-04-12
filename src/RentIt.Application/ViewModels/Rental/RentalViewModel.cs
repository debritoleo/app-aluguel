namespace RentIt.Application.ViewModels.Rental;

public class RentalViewModel
{
    public string Id { get; set; } = string.Empty;
    public string MotorcycleId { get; set; } = string.Empty;
    public string DeliverymanId { get; set; } = string.Empty;
    public DateTime StartDate { get; set; }
    public DateTime EndDate { get; set; }
    public DateTime ExpectedEndDate { get; set; }
    public int PlanDays { get; set; }
    public decimal DailyRate { get; set; }
}
