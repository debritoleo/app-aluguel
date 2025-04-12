using RentIt.Application.Commands.Rental;
using RentIt.Application.Common;
using RentIt.Domain.Aggregates.DeliverymanAggregate;
using RentIt.Domain.Aggregates.MotorcycleAggregate;
using RentIt.Domain.Repositories;

namespace RentIt.Application.Validators;

public class RentalCreationValidator
{
    private readonly IDeliverymanRepository _deliverymanRepository;
    private readonly IMotorcycleRepository _motorcycleRepository;
    private readonly IRentalRepository _rentalRepository;

    public RentalCreationValidator(
        IDeliverymanRepository deliverymanRepository,
        IMotorcycleRepository motorcycleRepository,
        IRentalRepository rentalRepository)
    {
        _deliverymanRepository = deliverymanRepository;
        _motorcycleRepository = motorcycleRepository;
        _rentalRepository = rentalRepository;
    }

    public async Task<Result<(Deliveryman, Motorcycle)>> ValidateAsync(CreateRentalRequest request, CancellationToken cancellationToken = default)
    {
        if (string.IsNullOrWhiteSpace(request.DeliverymanId))
            return Result<(Deliveryman, Motorcycle)>.Failure("Deliveryman ID is required.");

        if (string.IsNullOrWhiteSpace(request.MotorcycleId))
            return Result<(Deliveryman, Motorcycle)>.Failure("Motorcycle ID is required.");

        var deliveryman = await _deliverymanRepository.GetByIdAsync(request.DeliverymanId, cancellationToken);
        if (deliveryman is null)
            return Result<(Deliveryman, Motorcycle)>.Failure("Deliveryman not found.");

        if (!deliveryman.CanRentMotorcycle())
            return Result<(Deliveryman, Motorcycle)>.Failure("Deliveryman is not eligible to rent a motorcycle.");

        if (await _rentalRepository.HasActiveRentalAsync(deliveryman.Id, cancellationToken))
            return Result<(Deliveryman, Motorcycle)>.Failure("Deliveryman already has an active rental.");

        var motorcycle = await _motorcycleRepository.GetByIdAsync(request.MotorcycleId, cancellationToken);
        if (motorcycle is null)
            return Result<(Deliveryman, Motorcycle)>.Failure("Motorcycle not found.");

        return Result<(Deliveryman, Motorcycle)>.Success((deliveryman, motorcycle));
    }
}

