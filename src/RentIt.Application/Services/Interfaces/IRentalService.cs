using RentIt.Application.Commands.Rental;
using RentIt.Application.Common;

namespace RentIt.Application.Services.Interfaces;
public interface IRentalService
{
    Task<Result<string>> CreateAsync(CreateRentalRequest request, CancellationToken cancellationToken = default);
}
