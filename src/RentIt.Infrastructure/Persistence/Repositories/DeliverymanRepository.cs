using Microsoft.EntityFrameworkCore;
using RentIt.Domain.Aggregates.DeliverymanAggregate;
using RentIt.Domain.Repositories;

namespace RentIt.Infrastructure.Persistence.Repositories;

public class DeliverymanRepository : IDeliverymanRepository
{
    private readonly AppDbContext _context;

    public DeliverymanRepository(AppDbContext context)
    {
        _context = context;
    }

    public async Task AddAsync(Deliveryman deliveryman, CancellationToken cancellationToken = default)
    {
        await _context.Deliverymen.AddAsync(deliveryman, cancellationToken);
    }

    public async Task<bool> CnpjExistsAsync(string cnpj, CancellationToken cancellationToken = default)
    {
        return await _context.Deliverymen
            .AnyAsync(x => x.Cnpj.Value == cnpj, cancellationToken);
    }

    public async Task<bool> CnhNumberExistsAsync(string cnhNumber, CancellationToken cancellationToken = default)
    {
        return await _context.Deliverymen
            .AnyAsync(x => x.CnhNumber.Value == cnhNumber, cancellationToken);
    }

    public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        await _context.SaveChangesAsync(cancellationToken);
    }
}