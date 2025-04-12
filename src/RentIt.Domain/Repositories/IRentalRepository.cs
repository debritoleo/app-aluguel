using RentIt.Domain.Aggregates.RentalAggregate;

namespace RentIt.Domain.Repositories;
public interface IRentalRepository
{
    Task AddAsync(Rental rental, CancellationToken cancellationToken = default);
    Task<bool> HasActiveRentalAsync(string deliverymanId, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}
