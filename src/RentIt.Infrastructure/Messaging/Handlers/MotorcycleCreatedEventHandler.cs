using RentIt.Application.Messaging;
using RentIt.IntegrationEvents.Motorcycle;

namespace RentIt.Infrastructure.Messaging.Handlers;

public class MotorcycleCreatedEventHandler : IIntegrationEventHandler<MotorcycleCreatedEvent>
{
    private readonly AppDbContext _context;
    private readonly TimeProvider _timeProvider = TimeProvider.System;

    public MotorcycleCreatedEventHandler(AppDbContext context) => _context = context;

    public async Task HandleAsync(MotorcycleCreatedEvent @event, CancellationToken cancellationToken)
    {
        if (@event.Year == 2024)
        {
            _context.MotorcycleDenormalizeds.Add(new Domain.Aggregates.MotorcycleDenormalizedAggregate.MotorcycleDenormalized
            (
                @event.Identifier,
                @event.Year,
                @event.Model,
                @event.Plate,
                _timeProvider.GetUtcNow().UtcDateTime
            ));

            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}