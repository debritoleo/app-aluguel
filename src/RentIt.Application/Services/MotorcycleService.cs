using FluentValidation;
using RentIt.Application.Commands.Motorcycle;
using RentIt.Application.Common;
using RentIt.Application.Services.Interfaces;
using RentIt.Domain.Aggregates.MotorcycleAggregate;
using RentIt.Domain.Repositories;

namespace RentIt.Application.Services;
public class MotorcycleService : IMotorcycleService
{
    private readonly IMotorcycleRepository _repository;
    private readonly IValidator<CreateMotorcycleRequest> _validator;

    public MotorcycleService(IMotorcycleRepository repository, IValidator<CreateMotorcycleRequest> validator)
    {
        _repository = repository;
        _validator = validator;
    }

    public async Task<Result<string>> CreateAsync(CreateMotorcycleRequest request, CancellationToken cancellationToken = default)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return Result<string>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        if (await _repository.PlateExistsAsync(request.Placa, cancellationToken))
            return Result<string>.Failure("Placa já cadastrada.");

        var motorcycle = new Motorcycle(
            request.Identificador,
            request.Ano,
            request.Modelo,
            new Plate(request.Placa)
        );

        await _repository.AddAsync(motorcycle, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        return Result<string>.Success(motorcycle.Id.ToString());
    }
}
