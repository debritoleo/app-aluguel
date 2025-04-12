using FluentValidation;
using RentIt.Application.Commands.Rental;
using RentIt.Application.Common;
using RentIt.Application.Services.Interfaces;
using RentIt.Application.Validators;
using RentIt.Domain.Aggregates.RentalAggregate;
using RentIt.Domain.Repositories;

namespace RentIt.Application.Services;
public class RentalService : IRentalService
{
    private readonly IRentalRepository _rentalRepository;
    private readonly RentalCreationValidator _creationValidator;
    private readonly IValidator<CreateRentalRequest> _validator;
    private readonly TimeProvider _timeProvider;

    public RentalService(
        IRentalRepository rentalRepository,
        RentalCreationValidator creationValidator,
        IValidator<CreateRentalRequest> validator,
        TimeProvider timeProvider)
    {
        _rentalRepository = rentalRepository;
        _creationValidator = creationValidator;
        _validator = validator;
        _timeProvider = timeProvider;
    }

    public async Task<Result<string>> CreateAsync(CreateRentalRequest request, CancellationToken cancellationToken = default)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return Result<string>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        var businessValidation = await _creationValidator.ValidateAsync(request, cancellationToken);
        if (!businessValidation.IsSuccess)
            return Result<string>.Failure(businessValidation.Errors);

        var (deliveryman, motorcycle) = businessValidation.Value;
        var now = _timeProvider.GetUtcNow().UtcDateTime;

        var rental = Rental.Create(
            motorcycle.Id,
            deliveryman.Id,
            request.EndDate,
            request.ExpectedEndDate,
            now,
            deliveryman.CnhType);

        await _rentalRepository.AddAsync(rental, cancellationToken);
        await _rentalRepository.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(rental.Id);
    }
}
