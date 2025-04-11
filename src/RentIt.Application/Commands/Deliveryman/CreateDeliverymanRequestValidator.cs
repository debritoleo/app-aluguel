using FluentValidation;

namespace RentIt.Application.Commands.Deliveryman;
public class CreateDeliverymanRequestValidator : AbstractValidator<CreateDeliverymanRequest>
{
    public CreateDeliverymanRequestValidator()
    {
        RuleFor(x => x.Identificador).NotEmpty().MaximumLength(100);
        RuleFor(x => x.Nome).NotEmpty().MaximumLength(150);
        RuleFor(x => x.Cnpj).NotEmpty().Length(14);
        RuleFor(x => x.DataNascimento).NotEmpty();
        RuleFor(x => x.NumeroCnh).NotEmpty().MaximumLength(20);
        RuleFor(x => x.TipoCnh).IsInEnum();
    }
}
