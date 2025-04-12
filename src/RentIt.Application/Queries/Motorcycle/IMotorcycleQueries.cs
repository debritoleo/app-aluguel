using RentIt.Application.ViewModels.Motorcycle;
namespace RentIt.Application.Queries.Motorcycle;

public interface IMotorcycleQueries
{
    Task<IEnumerable<MotorcycleViewModel>> GetAllAsync(string? plate = null, CancellationToken cancellationToken = default);
    Task<MotorcycleViewModel?> GetByIdAsync(string id, CancellationToken cancellationToken = default);
}
