using RentIt.Application.Commands.Motorcycle;
using RentIt.Application.Common;

namespace RentIt.Application.Services.Interfaces;

public interface IMotorcycleService
{
    Task<Result<string>> CreateAsync(CreateMotorcycleRequest request, CancellationToken cancellationToken = default);
}
