using RentIt.Domain.Aggregates.MotorcycleAggregate;

namespace RentIt.Domain.Repositories;

public interface IMotorcycleRepository
{
    Task AddAsync(Motorcycle motorcycle, CancellationToken cancellationToken = default);
    Task<Motorcycle?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<Motorcycle?> GetByPlateAsync(string plate, CancellationToken cancellationToken = default);
    Task<bool> PlateExistsAsync(string plate, CancellationToken cancellationToken = default);
    Task SaveChangesAsync(CancellationToken cancellationToken = default);
}