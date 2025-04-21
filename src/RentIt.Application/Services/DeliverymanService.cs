using FluentValidation;
using RentIt.Application.Common;
using RentIt.Application.Requests.Deliveryman;
using RentIt.Application.Services.Interfaces;
using RentIt.Domain.Aggregates.DeliverymanAggregate;
using RentIt.Domain.Repositories;

namespace RentIt.Application.Services;

public class DeliverymanService : IDeliverymanService
{
    private readonly IDeliverymanRepository _repository;
    private readonly IValidator<CreateDeliverymanRequest> _validator;
    private readonly TimeProvider _timeProvider;

    public DeliverymanService(
        IDeliverymanRepository repository,
        IValidator<CreateDeliverymanRequest> validator,
        TimeProvider timeProvider)
    {
        _repository = repository;
        _validator = validator;
        _timeProvider = timeProvider;
    }

    public async Task<Result<string>> CreateAsync(CreateDeliverymanRequest request, CancellationToken cancellationToken = default)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return Result<string>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        if (await _repository.CnpjExistsAsync(request.Cnpj, cancellationToken))
            return Result<string>.Failure("CNPJ já cadastrado.");

        if (await _repository.CnhNumberExistsAsync(request.CnhNumber, cancellationToken))
            return Result<string>.Failure("Número da CNH já cadastrado.");

        var birthDate = new BirthDate(request.BirthDate);
        var now = _timeProvider.GetUtcNow().UtcDateTime;

        if (!birthDate.IsAdult(now))
            return Result<string>.Failure("O entregador deve ter pelo menos 18 anos.");

        var deliveryman = new Deliveryman(
            request.Identifier,
            request.Name,
            new Cnpj(request.Cnpj),
            birthDate,
            new CnhNumber(request.CnhNumber),
            CnhType.FromName(request.CnhType),
            now
        );

        await _repository.AddAsync(deliveryman, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(deliveryman.Id);
    }
}