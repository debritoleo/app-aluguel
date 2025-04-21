using FluentValidation;
using RentIt.Domain.Aggregates.DeliverymanAggregate;

namespace RentIt.Application.Requests.Deliveryman
{
    public class CreateDeliverymanRequestValidator : AbstractValidator<CreateDeliverymanRequest>
    {
        public CreateDeliverymanRequestValidator()
        {
            RuleFor(x => x.Identifier)
                .NotEmpty()
                .MaximumLength(100);

            RuleFor(x => x.Name)
                .NotEmpty()
                .MaximumLength(150);

            RuleFor(x => x.Cnpj)
                .NotEmpty()
                .Length(14);

            RuleFor(x => x.BirthDate)
                .NotEmpty();

            RuleFor(x => x.CnhNumber)
                .NotEmpty()
                .MaximumLength(20);

            RuleFor(x => x.CnhType)
                .NotEmpty()
                .Must(BeAValidCnhType)
                .WithMessage("Tipo de CNH inválido. Os valores válidos são: A, B ou AB.");
        }

        private bool BeAValidCnhType(string tipoCnh)
        {
            try
            {
                var _ = CnhType.FromName(tipoCnh);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
