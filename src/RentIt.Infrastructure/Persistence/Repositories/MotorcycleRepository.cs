using Microsoft.EntityFrameworkCore;
using RentIt.Domain.Aggregates.MotorcycleAggregate;
using RentIt.Domain.Repositories;

namespace RentIt.Infrastructure.Persistence.Repositories;

public class MotorcycleRepository : IMotorcycleRepository
{
    private readonly AppDbContext _context;

    public MotorcycleRepository(AppDbContext context) => _context = context;

    public async Task AddAsync(Motorcycle motorcycle, CancellationToken cancellationToken = default)
    {
        await _context.Motorcycles.AddAsync(motorcycle, cancellationToken);
    }

    public async Task<Motorcycle?> GetByIdAsync(string id, CancellationToken cancellationToken = default)
    {
        return await _context.Motorcycles.FindAsync([id, cancellationToken], cancellationToken: cancellationToken);
    }

    public async Task<Motorcycle?> GetByPlateAsync(string plate, CancellationToken cancellationToken = default)
    {
        return await _context.Motorcycles
                .FirstOrDefaultAsync(m => m.Plate.Value.ToLower() == plate.ToLower(), cancellationToken);
    }

    public async Task<bool> PlateExistsAsync(string plate, CancellationToken cancellationToken = default)
    {
        return await _context.Motorcycles.AnyAsync(m => m.Plate.Value.ToLower() == plate.ToLower());
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
