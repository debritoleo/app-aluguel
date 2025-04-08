using Microsoft.EntityFrameworkCore;
using RentIt.Domain.Aggregates.MotorcycleAggregate;
using RentIt.Domain.Repositories;

namespace RentIt.Infrastructure.Repositories;

public class MotorcycleRepository : IMotorcycleRepository
{
    private readonly AppDbContext _context;

    public MotorcycleRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Motorcycle motorcycle, CancellationToken cancellationToken = default)
    {
        await _context.Motorcycles.AddAsync(motorcycle, cancellationToken);
    }

    public async Task<Motorcycle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        return await _context.Motorcycles.FindAsync(new object[] { id }, cancellationToken);
    }

    public async Task<Motorcycle?> GetByPlateAsync(string plate, CancellationToken cancellationToken = default)
    {
        return await _context.Motorcycles
            .FirstOrDefaultAsync(x => x.Plate.Value == plate.ToUpper(), cancellationToken);
    }

    public async Task<bool> PlateExistsAsync(string plate, CancellationToken cancellationToken = default)
    {
        return await _context.Motorcycles.AnyAsync(x => x.Plate.Value == plate.ToUpper(), cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}
