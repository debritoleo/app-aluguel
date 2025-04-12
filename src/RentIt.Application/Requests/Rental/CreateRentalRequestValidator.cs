using FluentValidation;

namespace RentIt.Application.Commands.Rental;
public class CreateRentalRequestValidator : AbstractValidator<CreateRentalRequest>
{
    public CreateRentalRequestValidator()
    {
        RuleFor(x => x.DeliverymanId)
            .NotEmpty().WithMessage("Deliveryman ID is required.");

        RuleFor(x => x.MotorcycleId)
            .NotEmpty().WithMessage("Motorcycle ID is required.");

        RuleFor(x => x.EndDate)
            .GreaterThan(DateTime.UtcNow.Date).WithMessage("End date must be in the future.");

        RuleFor(x => x.ExpectedEndDate)
            .GreaterThanOrEqualTo(DateTime.UtcNow.Date.AddDays(1))
            .WithMessage("Expected end date must be at least the next day.");
    }
}
