using RentIt.Application.Messaging;
using RentIt.IntegrationEvents.Motorcycle;

namespace RentIt.Infrastructure.Messaging.Handlers;

public class MotorcycleCreatedEventHandler : IIntegrationEventHandler<MotorcycleCreatedEvent>
{
    private readonly AppDbContext _context;

    public MotorcycleCreatedEventHandler(AppDbContext context)
    {
        _context = context;
    }

    public async Task HandleAsync(MotorcycleCreatedEvent @event, CancellationToken cancellationToken)
    {
        //if (@event.Year == 2024)
        //{
            //_context.Motorcycles2024.Add(new Motorcycle2024Log
            //{
            //    MotorcycleId = @event.Id,
            //    Identifier = @event.Identifier,
            //    CreatedAt = DateTime.UtcNow
            //});

        //    await _context.SaveChangesAsync(cancellationToken);
        //}
    }
}