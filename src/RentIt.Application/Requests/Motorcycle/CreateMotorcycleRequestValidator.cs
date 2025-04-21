using FluentValidation;

namespace RentIt.Application.Commands.Motorcycle;
public class CreateMotorcycleRequestValidator : AbstractValidator<CreateMotorcycleRequest>
{
    public CreateMotorcycleRequestValidator()
    {
        RuleFor(x => x.Identifier).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Year).InclusiveBetween(2000, DateTime.UtcNow.Year + 1);
        RuleFor(x => x.Model).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Plate).NotEmpty().MaximumLength(10);
    }
}

