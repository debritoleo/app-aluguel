using Microsoft.EntityFrameworkCore;
using RentIt.Domain.Aggregates.RentalAggregate;
using RentIt.Domain.Repositories;

namespace RentIt.Infrastructure.Persistence.Repositories;

public class RentalRepository : IRentalRepository
{
    private readonly AppDbContext _context;

    public RentalRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Rental rental, CancellationToken cancellationToken = default)
    {
        await _context.Rentals.AddAsync(rental, cancellationToken);
    }

    public async Task<bool> HasActiveRentalAsync(string deliverymanId, CancellationToken cancellationToken = default)
    {
        return await _context.Rentals
            .AnyAsync(r => r.DeliverymanId == deliverymanId, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}