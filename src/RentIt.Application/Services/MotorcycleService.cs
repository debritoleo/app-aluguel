using FluentValidation;
using RentIt.Application.Commands.Motorcycle;
using RentIt.Application.Common;
using RentIt.Application.Messaging;
using RentIt.Application.Services.Interfaces;
using RentIt.Domain.Aggregates.MotorcycleAggregate;
using RentIt.Domain.Repositories;
using RentIt.IntegrationEvents.Motorcycle;

namespace RentIt.Application.Services;

public class MotorcycleService : IMotorcycleService
{
    private readonly IMotorcycleRepository _repository;
    private readonly IValidator<CreateMotorcycleRequest> _validator;
    private readonly IMessagePublisher _messagePublisher;

    public MotorcycleService(
        IMotorcycleRepository repository,
        IValidator<CreateMotorcycleRequest> validator,
        IMessagePublisher messagePublisher)
    {
        _repository = repository;
        _validator = validator;
        _messagePublisher = messagePublisher;
    }

    public async Task<Result<string>> CreateAsync(CreateMotorcycleRequest request, CancellationToken cancellationToken = default)
    {
        var validation = await _validator.ValidateAsync(request, cancellationToken);
        if (!validation.IsValid)
            return Result<string>.Failure(validation.Errors.Select(e => e.ErrorMessage));

        if (await _repository.PlateExistsAsync(request.Plate, cancellationToken))
            return Result<string>.Failure("Placa já cadastrada.");

        var motorcycle = new Motorcycle(
            request.Identifier,
            request.Year,
            request.Model,
            new Plate(request.Plate)
        );

        await _repository.AddAsync(motorcycle, cancellationToken);
        await _repository.SaveChangesAsync(cancellationToken);

        var @event = new MotorcycleCreatedEvent
        {
            Id = motorcycle.Id.ToString(),
            Identifier = motorcycle.Identifier,
            Year = motorcycle.Year,
            Model = motorcycle.Model,
            Plate = motorcycle.Plate.Value
        };

        await _messagePublisher.PublishAsync(@event, "motorcycles", cancellationToken);

        return Result<string>.Success(motorcycle.Id.ToString());
    }
}
