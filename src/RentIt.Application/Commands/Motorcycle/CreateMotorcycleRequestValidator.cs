using FluentValidation;

namespace RentIt.Application.Commands.Motorcycle;
public class CreateMotorcycleRequestValidator : AbstractValidator<CreateMotorcycleRequest>
{
    public CreateMotorcycleRequestValidator()
    {
        RuleFor(x => x.Identificador).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Ano).InclusiveBetween(2000, DateTime.UtcNow.Year + 1);
        RuleFor(x => x.Modelo).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Placa).NotEmpty().MaximumLength(10);
    }
}

